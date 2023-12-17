namespace SkillCentral.ApiClients;

public class SkillHttpClient : ISkillHttpClient
{
    private readonly SkillHttpClient http;

    public SkillHttpClient(SkillHttpClient http)
    {
        this.http = http;
    }
}
