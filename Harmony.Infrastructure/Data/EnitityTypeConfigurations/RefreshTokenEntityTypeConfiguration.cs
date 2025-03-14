using Harmony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harmony.Infrastructure.Data.EnitityTypeConfigurations;

public class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);

        //Relationship User -> RefreshToken is defined in UserEntityTypeConfiguration
    }
}
