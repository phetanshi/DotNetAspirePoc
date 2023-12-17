namespace SkillCentral.ApiClients;

public class EmployeeHttpClient : IEmployeeHttpClient
{
    private readonly HttpClient http;

    public EmployeeHttpClient(HttpClient http)
    {
        this.http = http;
    }
}
