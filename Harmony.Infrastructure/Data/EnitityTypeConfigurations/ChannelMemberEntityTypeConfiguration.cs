using Harmony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harmony.Infrastructure.Data.EnitityTypeConfigurations;

public class ChannelMemberEntityTypeConfiguration : IEntityTypeConfiguration<ChannelMember>
{
    public void Configure(EntityTypeBuilder<ChannelMember> builder)
    {
        builder.ToTable("ChannelMembers");

        // PK
        builder.HasKey(cm => new { cm.ChannelId, cm.UserId });

        // Properties
        builder.Property(cm => cm.UserId)
                .IsRequired();

        builder.Property(cm => cm.ChannelId)
                .IsRequired();

        // one Channel -> many ChannelMembers
        builder.HasOne(cm => cm.Channel)
               .WithMany(c => c.ChannelMembers)
               .HasForeignKey(cm => cm.ChannelId);

        // one User -> many ChannelMembers
        builder.HasOne(cm => cm.User)
               .WithMany(u => u.ChannelMembers)
               .HasForeignKey(cm => cm.UserId);
    }
}
