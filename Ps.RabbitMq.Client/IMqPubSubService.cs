namespace Ps.RabbitMq.Client;

public interface IMqPubSubService
{
    #region Publish
    Task PublishFanoutAsync<T>(T message, string exchangeName = MQConstants.FANOUT_EXCHANGE_NAME) where T : class;
    Task PublishDirectAsync<T>(T message, string routingKey, string exchangeName = MQConstants.DIRECT_EXCHANGE_NAME) where T : class;
    Task PublishTopicAsync<T>(T message, string routingKey, string exchangeName = MQConstants.TOPIC_EXCHANGE_NAME) where T : class;
    #endregion

    #region Consume

    Task ConsumeFanoutAsync<TReturn>(Action<TReturn> callback, string exchangeName = MQConstants.FANOUT_EXCHANGE_NAME) where TReturn : class;
    Task ConsumeDirectAsync<TReturn>(string routingKey, Action<TReturn> callback, string exchangeName = MQConstants.DIRECT_EXCHANGE_NAME) where TReturn : class;
    Task ConsumeTopicAsync<TReturn>(string routingKey, Action<TReturn> callback, string exchangeName = MQConstants.TOPIC_EXCHANGE_NAME) where TReturn : class;
    #endregion
}
