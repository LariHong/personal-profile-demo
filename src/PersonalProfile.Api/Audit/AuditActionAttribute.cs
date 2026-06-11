using Microsoft.AspNetCore.Mvc.Filters;

namespace PersonalProfile.Api.Audit;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class AuditActionAttribute(string actionName) : Attribute, IFilterFactory
{
    public string ActionName { get; } = actionName;

    public bool IsReusable => false;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var filter = serviceProvider.GetRequiredService<AuditActionFilter>();
        filter.ActionName = ActionName;
        return filter;
    }
}
