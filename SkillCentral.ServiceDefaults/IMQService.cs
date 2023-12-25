namespace SkillCentral.ServiceDefaults
{
    public interface IMQService
    {
        TReturn GetResponse<T, TReturn>(T messageToPublish);
        void GetRequest<T, TResponse>(string queueName, Func<T, TResponse> businessLogic);
        void SendRequest<T>(T messageToPublish, string tobeSendQueueName, string correlationId, string replyQueueName = "");
    }
}
