using Harmony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harmony.Infrastructure.Data.EnitityTypeConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // PK
        builder.HasKey(u => u.Id);

        // one User -> many RefreshToken
        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User);
    }
}
