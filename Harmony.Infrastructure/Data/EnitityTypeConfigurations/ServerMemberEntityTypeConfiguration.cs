using Harmony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harmony.Infrastructure.Data.EnitityTypeConfigurations;

public class ServerMemberEntityTypeConfiguration : IEntityTypeConfiguration<ServerMember>
{
    public void Configure(EntityTypeBuilder<ServerMember> builder)
    {
        builder.ToTable("ServerMembers");

        // PK
        builder.HasKey(sm => new { sm.ServerId, sm.UserId });

        // Properties
        builder.Property(sm => sm.UserId)
                .IsRequired();

        builder.Property(sm => sm.ServerId)
                .IsRequired();

        builder.Property(sm => sm.UserRole)
            .HasConversion<string>();

        // one Server -> many ServerMembers
        builder.HasOne(sm => sm.Server)
               .WithMany(s => s.ServerMembers)
               .HasForeignKey(sm => sm.ServerId);

        // one User -> many ServerMembers
        builder.HasOne(sm => sm.User)
               .WithMany(u => u.ServerMembers)
               .HasForeignKey(sm => sm.UserId);
    }
}
