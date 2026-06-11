namespace PersonalProfile.Api.Audit;

public sealed record AuditEntry(
    string ActionName,
    string HttpMethod,
    string Path,
    int StatusCode,
    TimeSpan Duration,
    string TraceId);
