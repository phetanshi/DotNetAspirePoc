using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ps.RabbitMq.Client
{
    public class MqPubSubService(IConnection connection, MqUtil mqUtil) : IMqPubSubService
    {
        private readonly IModel _channel = connection.CreateModel();

        #region PublicMethods

        #region Publish
        /// <summary>
        /// Publishes message using fanout exchange type
        /// </summary>
        /// <typeparam name="T">type of message</typeparam>
        /// <param name="message">message to be published</param>
        /// <param name="exchangeName"></param>
        public async Task PublishFanoutAsync<T>(T message, string exchangeName = MQConstants.FANOUT_EXCHANGE_NAME) where T : class
        {
            PublishInput<T> publishInput = new PublishInput<T>();
            publishInput.Message = message;
            publishInput.ExchangeName = exchangeName;
            publishInput.ExchangeType = ExchangeType.Fanout;
            publishInput.RouteKey = "";
            await PublishAsync(publishInput);
        }

        /// <summary>
        /// Publishes message using fanout exchange type
        /// </summary>
        /// <typeparam name="T">type of message</typeparam>
        /// <param name="message">message to be published</param>
        /// <param name="exchangeName"></param>
        public async Task PublishDirectAsync<T>(T message, string routingKey, string exchangeName = MQConstants.DIRECT_EXCHANGE_NAME) where T : class
        {
            PublishInput<T> publishInput = new PublishInput<T>();
            publishInput.Message = message;
            publishInput.ExchangeName = exchangeName;
            publishInput.ExchangeType = ExchangeType.Direct;
            publishInput.RouteKey = routingKey;
            await PublishAsync(publishInput);
        }

        /// <summary>
        /// Publishes message using fanout exchange type
        /// </summary>
        /// <typeparam name="T">type of message</typeparam>
        /// <param name="message">message to be published</param>
        /// <param name="exchangeName"></param>
        public async Task PublishTopicAsync<T>(T message, string routingKey, string exchangeName = MQConstants.TOPIC_EXCHANGE_NAME) where T : class
        {
            PublishInput<T> publishInput = new PublishInput<T>();
            publishInput.Message = message;
            publishInput.ExchangeName = exchangeName;
            publishInput.ExchangeType = ExchangeType.Topic;
            publishInput.RouteKey = routingKey;
            await PublishAsync(publishInput);
        }
        #endregion

        #region Consume

        /// <summary>
        /// Consumes message from fanout exchanges
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="callback">Call back method to execute business logic on the message from queue</param>
        /// <param name="exchangeName"></param>
        public async Task ConsumeFanoutAsync<TReturn>(Action<TReturn> callback, string exchangeName = MQConstants.FANOUT_EXCHANGE_NAME) where TReturn : class
        {
            await ConsumeAsync<TReturn>(routingKey: "", callback, exchangeName);
        }

        /// <summary>
        /// Consumes message from direct exchanges
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="callback">Call back method to execute business logic on the message from queue</param>
        /// <param name="exchangeName"></param>
        public async Task ConsumeDirectAsync<TReturn>(string routingKey, Action<TReturn> callback, string exchangeName = MQConstants.DIRECT_EXCHANGE_NAME) where TReturn : class
        {
            await ConsumeAsync<TReturn>(routingKey: routingKey, callback: callback, exchangeName: exchangeName, exchangeType: ExchangeType.Direct);
        }

        /// <summary>
        /// Consumes message from topic exchanges
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="callback">Call back method to execute business logic on the message from queue</param>
        /// <param name="exchangeName"></param>
        public async Task ConsumeTopicAsync<TReturn>(string routingKey, Action<TReturn> callback, string exchangeName = MQConstants.TOPIC_EXCHANGE_NAME) where TReturn : class
        {
            await ConsumeAsync<TReturn>(routingKey: routingKey, callback: callback, exchangeName: exchangeName, exchangeType: ExchangeType.Topic);
        }

        #endregion

        #endregion

        #region PrivateMethods

        private async Task PublishAsync<T>(PublishInput<T> publishInput)
        {
            byte[]? encodedBody = null;

            if (mqUtil.IsPrimitiveType<T>())
            {
                InputMessage<T> messageObj = new InputMessage<T>();
                messageObj.Message = publishInput.Message;
                encodedBody = mqUtil.GetEncodedMessage(messageObj);
            }
            else
                encodedBody = mqUtil.GetEncodedMessage(publishInput.Message);

            _channel.ExchangeDeclare(exchange: publishInput.ExchangeName, type: publishInput.ExchangeType);
            _channel.BasicPublish(exchange: publishInput.ExchangeName, routingKey: publishInput.RouteKey, basicProperties: null, body: encodedBody);

            await Task.CompletedTask;
        }
        private async Task ConsumeAsync<TReturn>(string routingKey, Action<TReturn> callback, string exchangeName = MQConstants.FANOUT_EXCHANGE_NAME, string exchangeType = ExchangeType.Fanout)
        {
            //Not needed in Consumer. However no harm in having it here. If exchange already exist, it will simply ignores it otherwise create it
            _channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
            var queueName = _channel.QueueDeclare().QueueName;
            var consumer = new EventingBasicConsumer(_channel);
            _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);
            consumer.Received += (sender, e) => mqUtil.OnReceived<TReturn>(sender, e, ConsumeMessage: (msg, props) => callback(msg));
            _channel.BasicConsume(queue: queueName, autoAck: false, consumer);
            await Task.CompletedTask;
        }

        #endregion
    }
}
