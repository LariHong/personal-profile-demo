namespace PersonalProfile.Api.Models;

public sealed class PersonalProfileEntity
{
    public int Id { get; set; }

    public string NationalId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public DateOnly Birthday { get; set; }

    public string City { get; set; } = string.Empty;

    public string District { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}
