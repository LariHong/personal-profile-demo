namespace PersonalProfile.Api.Dtos;

public sealed record PersonalProfileDto(
    int Id,
    string NationalId,
    string Name,
    string Gender,
    DateOnly Birthday,
    string City,
    string District,
    string Address,
    string Phone,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
