using Harmony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harmony.Infrastructure.Data.EnitityTypeConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        // PK
        builder.HasKey(u => u.Id);

        // Properties
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        // one User -> many RefreshToken
        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId);

        // many Users -> many Servers
        builder.HasMany(u => u.Servers)
            .WithMany(s => s.Users)
            .UsingEntity<ServerMember>(
                left => left.HasOne(e => e.Server).WithMany(e => e.ServerMembers).HasForeignKey(sm => sm.ServerId),
                right => right.HasOne(e => e.User).WithMany(e => e.ServerMembers).HasForeignKey(sm => sm.UserId));

        // many Users -> many Channels
        builder.HasMany(u => u.Channels)
            .WithMany(c => c.Users)
            .UsingEntity<ChannelMember>(
                left => left.HasOne(e => e.Channel).WithMany(e => e.ChannelMembers).HasForeignKey(cm => cm.ChannelId),
                right => right.HasOne(e => e.User).WithMany(e => e.ChannelMembers).HasForeignKey(cm => cm.UserId));

        // one User -> many Messages
        builder.HasMany(u => u.Messages)
            .WithOne(m => m.User)
            .HasForeignKey(m => m.UserId);
    }
}
