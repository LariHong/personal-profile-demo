namespace PersonalProfile.Api.Audit;

public sealed class LoggingAuditLogger(ILogger<LoggingAuditLogger> logger) : IAuditLogger
{
    public void Write(AuditEntry entry)
    {
        logger.LogInformation(
            "Audit action {ActionName} {HttpMethod} {Path} completed with {StatusCode} in {DurationMs} ms. TraceId: {TraceId}",
            entry.ActionName,
            entry.HttpMethod,
            entry.Path,
            entry.StatusCode,
            entry.Duration.TotalMilliseconds,
            entry.TraceId);
    }
}
