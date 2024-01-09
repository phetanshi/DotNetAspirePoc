namespace SkillCentral.ApiClients
{
    public abstract class ClientBase(HttpClient http, ILogger logger)
    {
        protected HttpClient Http { get; } = http;
        protected ILogger Logger { get; } = logger;
    }
}
