using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Ps.RabbitMq.Client;

public class MqUtil
{
    internal byte[] GetEncodedMessage<T>(T message)
    {
        string serializedMessage = SerializedMessage(message);
        var encodedBody = Encoding.UTF8.GetBytes(serializedMessage);
        return encodedBody;
    }

    internal void OnReceived<TReturn>(object? sender, BasicDeliverEventArgs e, Action<TReturn, IDictionary<string, string>> ConsumeMessage)
    {
        TReturn response = Deserialize<TReturn>(e);

        var values = new Dictionary<string, string>();
        values.Add("ReplyTo", e.BasicProperties.ReplyTo);
        values.Add("CorrelationId", e.BasicProperties.CorrelationId);

        ConsumeMessage(response, values);

        if (sender is not null)
        {
            var channel = ((EventingBasicConsumer)sender).Model;
            channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
        }
    }

    internal async Task OnReceivedAsync<TReturn>(IModel channel, BasicDeliverEventArgs e, Action<TReturn, IDictionary<string, string>> ConsumeMessage)
    {
        TReturn response = Deserialize<TReturn>(e);
        var values = new Dictionary<string, string>();
        values.Add("ReplyTo", e.BasicProperties.ReplyTo);
        values.Add("CorrelationId", e.BasicProperties.CorrelationId);
        ConsumeMessage(response, values);
        channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
        await Task.CompletedTask;
    }
    internal bool IsPrimitiveType<T>()
    {
        Type type = typeof(T);

        var isTrue = type.IsPrimitive ||
               type == typeof(string) ||
               type == typeof(decimal) ||
               type == typeof(DateTime) ||
               type == typeof(TimeSpan) ||
               type == typeof(Guid) ||
               type.IsEnum;

        return isTrue;
    }
    internal string SerializedMessage<T>(T message)
    {
        if (typeof(T) == typeof(string))
            return message?.ToString() ?? "";
        else
            return JsonSerializer.Serialize(message);
    }

    internal IBasicProperties GetBasicProperties<T>(IModel channel, PublishInput<T> publishInput)
    {
        IBasicProperties props = channel.CreateBasicProperties();
        if (!string.IsNullOrWhiteSpace(publishInput.CorrelationId))
            props.CorrelationId = publishInput.CorrelationId;
        if (!string.IsNullOrWhiteSpace(publishInput.ReplyQueueName))
            props.ReplyTo = publishInput.ReplyQueueName;
        return props;
    }
    internal T Deserialize<T>(BasicDeliverEventArgs e)
    {
        T returnVal = default;
        var message = ReadMessageAsString(e);
        message = message.Replace("\\u0022", "\"");
        if (IsPrimitiveType<T>())
        {
            var obj = Deserialize<InputMessage<T>>(message);
            returnVal = obj.Message;
        }
        else
            returnVal = JsonSerializer.Deserialize<T>(message);
        return returnVal;
    }
    internal T Deserialize<T>(string message)
    {
        T returnVal = default;
        message = message.Replace("\\u0022", "\"");

        if (IsPrimitiveType<T>())
            returnVal = (T)Convert.ChangeType(message, typeof(T));
        else
            returnVal = JsonSerializer.Deserialize<T>(message);
        return returnVal;
    }

    private string ReadMessageAsString(BasicDeliverEventArgs e)
    {
        var body = e.Body.ToArray();
        return Encoding.UTF8.GetString(body);
    }
}
