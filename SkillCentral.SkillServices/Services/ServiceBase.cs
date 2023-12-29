namespace SkillCentral.SkillServices.Services;

public abstract class ServiceBase
{
    public ServiceBase(IHttpContextAccessor context)
    {
        AppHttpContext = context;
    }
    public IHttpContextAccessor AppHttpContext { get; }

    protected string GetLoginUserId()
    {
        if (AppHttpContext == null)
            return "Not-Login";

        return AppHttpContext.HttpContext?.User?.Identity?.Name ?? "Not-Login";
    }
}
