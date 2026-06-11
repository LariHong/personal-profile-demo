using System.ComponentModel.DataAnnotations;
using PersonalProfile.Api.Validation;

namespace PersonalProfile.Api.Dtos;

public sealed class UpsertPersonalProfileRequest
{
    [Required]
    [TaiwanNationalId]
    public string NationalId { get; init; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "性別只能是 Male、Female 或 Other。")]
    public string Gender { get; init; } = string.Empty;

    [Required]
    public DateOnly? Birthday { get; init; }

    [Required]
    [StringLength(20)]
    public string City { get; init; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string District { get; init; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string Address { get; init; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(30)]
    public string Phone { get; init; } = string.Empty;
}
