namespace PersonalProfile.Api.Audit;

public interface IAuditLogger
{
    void Write(AuditEntry entry);
}
