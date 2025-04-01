using Harmony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harmony.Infrastructure.Data.EnitityTypeConfigurations;

public class ChannelEntityTypeConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.ToTable("Channels");

        // PK
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.ServerId)
            .IsRequired();

        builder.Property(c => c.ChannelName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.ChannelType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(c => c.CreatedDate)
            .IsRequired();

        // one Channel -> one Server BUT one Server -> many Channels
        builder.HasOne(c => c.Server)
            .WithMany(s => s.Channels)
            .HasForeignKey(c => c.ServerId);

        // many Channels -> many Users
        builder.HasMany(c => c.Users)
            .WithMany(u => u.Channels)
            .UsingEntity<ChannelMember>(
                left => left.HasOne(e => e.User).WithMany(e => e.ChannelMembers).HasForeignKey(cm => cm.UserId),
                right => right.HasOne(e => e.Channel).WithMany(e => e.ChannelMembers).HasForeignKey(cm => cm.ChannelId));

        // one Channel -> many Messages
        builder.HasMany(c => c.Messages)
            .WithOne(m => m.Channel)
            .HasForeignKey(m => m.ChannelId);
    }
}
