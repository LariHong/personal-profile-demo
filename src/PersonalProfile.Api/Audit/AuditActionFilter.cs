using Microsoft.AspNetCore.Mvc.Filters;

namespace PersonalProfile.Api.Audit;

public sealed class AuditActionFilter(IAuditLogger auditLogger, ISystemClock clock) : IAsyncActionFilter
{
    public string ActionName { get; set; } = string.Empty;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var startedAt = clock.UtcNow;
        var executedContext = await next();
        var duration = clock.UtcNow - startedAt;

        var entry = new AuditEntry(
            ActionName,
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path,
            executedContext.HttpContext.Response.StatusCode,
            duration,
            context.HttpContext.TraceIdentifier);

        auditLogger.Write(entry);
    }
}
