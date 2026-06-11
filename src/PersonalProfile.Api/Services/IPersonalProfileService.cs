using PersonalProfile.Api.Dtos;

namespace PersonalProfile.Api.Services;

public interface IPersonalProfileService
{
    Task<IReadOnlyList<PersonalProfileDto>> SearchAsync(string? keyword, CancellationToken cancellationToken);

    Task<PersonalProfileDto?> GetAsync(int id, CancellationToken cancellationToken);

    Task<PersonalProfileDto> CreateAsync(UpsertPersonalProfileRequest request, CancellationToken cancellationToken);

    Task<PersonalProfileDto?> UpdateAsync(int id, UpsertPersonalProfileRequest request, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
