using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using PersonalProfile.Api.Data;
using PersonalProfile.Api.Dtos;
using PersonalProfile.Api.Models;

namespace PersonalProfile.Api.Services;

public sealed class PersonalProfileService(AppDbContext db) : IPersonalProfileService
{
    public async Task<IReadOnlyList<PersonalProfileDto>> SearchAsync(string? keyword, CancellationToken cancellationToken)
    {
        var query = db.PersonalProfiles.AsNoTracking();
        var term = keyword?.Trim();

        if (!string.IsNullOrWhiteSpace(term))
        {
            query = query.Where(x =>
                x.NationalId.Contains(term) ||
                x.Name.Contains(term) ||
                x.City.Contains(term) ||
                x.District.Contains(term) ||
                x.Phone.Contains(term));
        }

        return await query
            .OrderByDescending(x => x.UpdatedAt)
            .Select(x => ToDto(x))
            .ToListAsync(cancellationToken);
    }

    public async Task<PersonalProfileDto?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return await db.PersonalProfiles
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => ToDto(x))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<PersonalProfileDto> CreateAsync(UpsertPersonalProfileRequest request, CancellationToken cancellationToken)
    {
        var normalizedNationalId = NormalizeNationalId(request.NationalId);
        await EnsureNationalIdIsUniqueAsync(normalizedNationalId, exceptId: null, cancellationToken);

        var now = DateTimeOffset.UtcNow;
        var profile = new PersonalProfileEntity
        {
            NationalId = normalizedNationalId,
            Name = request.Name.Trim(),
            Gender = request.Gender,
            Birthday = request.Birthday!.Value,
            City = request.City.Trim(),
            District = request.District.Trim(),
            Address = request.Address.Trim(),
            Phone = request.Phone.Trim(),
            CreatedAt = now,
            UpdatedAt = now
        };

        db.PersonalProfiles.Add(profile);
        await SaveChangesAsync(cancellationToken);
        return ToDto(profile);
    }

    public async Task<PersonalProfileDto?> UpdateAsync(int id, UpsertPersonalProfileRequest request, CancellationToken cancellationToken)
    {
        var profile = await db.PersonalProfiles.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (profile is null)
        {
            return null;
        }

        var normalizedNationalId = NormalizeNationalId(request.NationalId);
        await EnsureNationalIdIsUniqueAsync(normalizedNationalId, id, cancellationToken);

        profile.NationalId = normalizedNationalId;
        profile.Name = request.Name.Trim();
        profile.Gender = request.Gender;
        profile.Birthday = request.Birthday!.Value;
        profile.City = request.City.Trim();
        profile.District = request.District.Trim();
        profile.Address = request.Address.Trim();
        profile.Phone = request.Phone.Trim();
        profile.UpdatedAt = DateTimeOffset.UtcNow;

        await SaveChangesAsync(cancellationToken);
        return ToDto(profile);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var profile = await db.PersonalProfiles.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (profile is null)
        {
            return false;
        }

        db.PersonalProfiles.Remove(profile);
        await SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task EnsureNationalIdIsUniqueAsync(string nationalId, int? exceptId, CancellationToken cancellationToken)
    {
        var exists = await db.PersonalProfiles.AnyAsync(x =>
            x.NationalId == nationalId && (!exceptId.HasValue || x.Id != exceptId.Value),
            cancellationToken);

        if (exists)
        {
            throw new DuplicateNationalIdException();
        }
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            throw new DuplicateNationalIdException();
        }
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException exception)
    {
        return exception.GetBaseException() is SqlException { Number: 2601 or 2627 };
    }

    private static string NormalizeNationalId(string nationalId) => nationalId.Trim().ToUpperInvariant();

    private static PersonalProfileDto ToDto(PersonalProfileEntity profile)
    {
        return new PersonalProfileDto(
            profile.Id,
            profile.NationalId,
            profile.Name,
            profile.Gender,
            profile.Birthday,
            profile.City,
            profile.District,
            profile.Address,
            profile.Phone,
            profile.CreatedAt,
            profile.UpdatedAt);
    }
}
