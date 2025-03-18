using Harmony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harmony.Infrastructure.Data.EnitityTypeConfigurations;

public class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // PK
        builder.HasKey(rt => rt.Id);

        // Properties
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(rt => rt.ExpiresAt)
            .IsRequired();

        builder.Property(rt => rt.IsRevoked)
            .IsRequired();

        builder.Property(rt => rt.UserId)
            .IsRequired();

        //Relationship User -> RefreshToken is defined in UserEntityTypeConfiguration
        //builder.HasOne(rt => rt.User)
        //    .WithMany(u => u.RefreshTokens)
        //    .HasForeignKey(rt => rt.UserId);
    }
}
