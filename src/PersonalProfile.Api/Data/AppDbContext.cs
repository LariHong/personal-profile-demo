using Microsoft.EntityFrameworkCore;
using PersonalProfile.Api.Models;

namespace PersonalProfile.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<PersonalProfileEntity> PersonalProfiles => Set<PersonalProfileEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var profile = modelBuilder.Entity<PersonalProfileEntity>();

        profile.HasKey(x => x.Id);
        profile.HasIndex(x => x.NationalId).IsUnique();
        profile.Property(x => x.NationalId).HasMaxLength(10).IsRequired();
        profile.Property(x => x.Name).HasMaxLength(80).IsRequired();
        profile.Property(x => x.Gender).HasMaxLength(12).IsRequired();
        profile.Property(x => x.City).HasMaxLength(20).IsRequired();
        profile.Property(x => x.District).HasMaxLength(20).IsRequired();
        profile.Property(x => x.Address).HasMaxLength(160).IsRequired();
        profile.Property(x => x.Phone).HasMaxLength(30).IsRequired();
    }
}
