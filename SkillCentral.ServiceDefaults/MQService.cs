using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace SkillCentral.ServiceDefaults
{
    public class MQService : IMQService
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;
        public MQService(IConnection connection)
        {
            _connection = connection;
            _channel = connection.CreateModel();
        }


        public TReturn GetResponse<T, TReturn>(T messageToPublish)
        {
            string replyQueueName = _channel.QueueDeclare().QueueName;
            var requestQueueName = typeof(TReturn).FullName;

            var correlationId = Guid.NewGuid().ToString();
            EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            TReturn returnVal = default;

            var consumer = new EventingBasicConsumer(_channel);
            _channel.BasicConsume(queue: replyQueueName, autoAck: false, consumer: consumer);

            void ConsumeMessage(TReturn response, IDictionary<string, string> vals)
            {
                var str = JsonSerializer.Serialize(response);
                returnVal = response;
            }
            consumer.Received += (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    OnResponseReceived<T, TReturn>(model, ea, ConsumeMessage);
                    waitHandle.Set();
                }
            };
            SendRequest(messageToPublish, requestQueueName, correlationId, replyQueueName);
            waitHandle.WaitOne();
            return returnVal;
        }
        public void GetRequest<T, TResponse>(string queueName, Func<T, TResponse> businessLogic)
        {
            _channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(_channel);
            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            void ConsumeMessage(T resp, IDictionary<string, string> vals)
            {
                var response = businessLogic(resp);
                SendRequest(response, vals["ReplyTo"], vals["CorrelationId"]);
            }
            consumer.Received += (model, ea) =>
            {
                OnResponseReceived<T, T>(model, ea, ConsumeMessage);
            };
        }
        public void SendRequest<T>(T messageToPublish, string tobeSendQueueName, string correlationId, string replyQueueName = "")
        {
            var serializedMessage = JsonSerializer.Serialize(messageToPublish);
            var responseBytes = Encoding.UTF8.GetBytes(serializedMessage);

            var props = _channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            if (!string.IsNullOrWhiteSpace(replyQueueName))
                props.ReplyTo = replyQueueName;

            _channel.BasicPublish(exchange: "",
                                routingKey: tobeSendQueueName,
                                basicProperties: props,
                                body: responseBytes);
        }


        #region PrivateMethods
        private T Deserialize<T>(byte[] buffer, Type type)
        {
            var str = System.Text.Encoding.UTF8.GetString(buffer);
            str = str.Replace("\\u0022", "\"");
            return JsonSerializer.Deserialize<T>(str);
        }
        void OnResponseReceived<T, TReturn>(object? model, BasicDeliverEventArgs ea, Action<TReturn, IDictionary<string, string>> ConsumeMessage)
        {
            var body = ea.Body.ToArray();
            TReturn response = default;

            if (typeof(TReturn) == typeof(string))
                response = (TReturn)(object)System.Text.Encoding.UTF8.GetString(body).Replace("\"", "").Replace("\\u0022", "\"");
            else
                response = Deserialize<TReturn>(body, typeof(T));

            var vals = new Dictionary<string, string>();
            vals.Add("ReplyTo", ea.BasicProperties.ReplyTo);
            vals.Add("CorrelationId", ea.BasicProperties.CorrelationId);

            ConsumeMessage(response, vals);

            if (model is not null)
            {
                var channel = ((EventingBasicConsumer)model).Model;
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
        }
        #endregion
    }
}
