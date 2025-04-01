using Harmony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harmony.Infrastructure.Data.EnitityTypeConfigurations;

public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");

        //PK
        builder.HasKey(m => m.Id);

        //Properties
        builder.Property(m => m.MessageContent)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(m => m.UserId)
            .IsRequired();

        builder.Property(m => m.SentAt)
            .IsRequired();

        // one Message -> one User BUT one User -> many Messages
        builder.HasOne(m => m.User)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.UserId);

        // one Message -> one Channel BUT one Channel -> many Messages
        builder.HasOne(m => m.Channel)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChannelId);
    }
}
