namespace Ps.RabbitMq.Client;

public interface IMqRequestService
{
    Task<TReturn> GetResponseAsync<T, TReturn>(T message) where T : class where TReturn : class;
    Task GetRequestAsync<T, TReturn>(Func<T, TReturn> businessLogic, string queueName = "") where T : class where TReturn : class;
    Task RespondAsync<T>(PublishInput<T> publishInput) where T : class;
}
