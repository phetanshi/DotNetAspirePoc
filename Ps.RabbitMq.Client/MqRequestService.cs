using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ps.RabbitMq.Client;

public class MqRequestService(IConnection connection, MqUtil mqUtil) : IMqRequestService
{
    private readonly IModel _channel = connection.CreateModel();

    public async Task<TReturn> GetResponseAsync<T, TReturn>(T message) where T : class where TReturn : class
    {
        TReturn returnVal = default;
        string replyQueueName = _channel.QueueDeclare().QueueName;
        var correlationId = Guid.NewGuid().ToString();

        EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        var consumer = new EventingBasicConsumer(_channel);

        _channel.BasicConsume(queue: replyQueueName, autoAck: false, consumer: consumer);
        consumer.Received += (model, ea) =>
        {
            if (ea.BasicProperties.CorrelationId == correlationId)
            {
                mqUtil.OnReceived<TReturn>(model, ea, (resp, values) => returnVal = resp);
                waitHandle.Set();
            }
        };

        PublishInput<T> publishInput = new PublishInput<T>();
        publishInput.Message = message;
        publishInput.RouteKey = typeof(TReturn).FullName ?? "";
        publishInput.CorrelationId = correlationId;
        publishInput.ReplyQueueName = replyQueueName;

        RespondAsync(publishInput);

        waitHandle.WaitOne();
        return await Task.FromResult(returnVal);
    }
    public async Task GetRequestAsync<T, TReturn>(Func<T, TReturn> businessLogic, string queueName = "") where T : class where TReturn : class
    {
        if (string.IsNullOrEmpty(queueName))
        {
            queueName = typeof(TReturn).FullName ?? "request_queue";
        }
        _channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        var consumer = new EventingBasicConsumer(_channel);
        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

        consumer.Received += (model, ea) =>
        {
            mqUtil.OnReceived<T>(model, ea, (resp, vals) =>
            {
                Console.WriteLine($"Request received");
                TReturn response = businessLogic(resp);

                PublishInput<TReturn> publishInput = new PublishInput<TReturn>();
                publishInput.Message = response;
                publishInput.RouteKey = vals["ReplyTo"];
                publishInput.CorrelationId = vals["CorrelationId"];
                RespondAsync(publishInput).GetAwaiter().GetResult();
            });
        };

        await Task.CompletedTask;
    }
    public async Task RespondAsync<T>(PublishInput<T> publishInput) where T : class
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

        var props = mqUtil.GetBasicProperties<T>(_channel, publishInput);

        _channel.BasicPublish(exchange: publishInput.ExchangeName,
                            routingKey: publishInput.RouteKey,
                            basicProperties: props,
                            body: encodedBody);

        await Task.CompletedTask;
    }
}
