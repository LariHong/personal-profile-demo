namespace PersonalProfile.Api.Audit;

public interface ISystemClock
{
    DateTimeOffset UtcNow { get; }
}
