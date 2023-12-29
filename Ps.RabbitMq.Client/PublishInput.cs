namespace Ps.RabbitMq.Client;

public class PublishInput<T>
{
    public PublishInput()
    {
        ExchangeName = "";
        RouteKey = "";
        ExchangeType = "";
    }
    public T Message { get; set; }
    public string RouteKey { get; set; }
    public string ExchangeName { get; set; }
    public string ExchangeType { get; set; }
    public string CorrelationId { get; set; }
    public string ReplyQueueName { get; set; }
}
