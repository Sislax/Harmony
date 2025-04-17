using Harmony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harmony.Infrastructure.Data.EnitityTypeConfigurations;

public class ServerEntityTypeConfiguration : IEntityTypeConfiguration<Server>
{
    public void Configure(EntityTypeBuilder<Server> builder)
    {
        builder.ToTable("Servers");

        //PK
        builder.HasKey(s => s.Id);

        //Properties
        builder.Property(s => s.ServerName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.CreatedDate)
            .IsRequired();

        // many Servers -> many Users
        builder.HasMany(s => s.Users)
            .WithMany(u => u.Servers)
            .UsingEntity<ServerMember>(
                left => left.HasOne(e => e.User).WithMany(e => e.ServerMembers).HasForeignKey(cm => cm.UserId),
                right => right.HasOne(e => e.Server).WithMany(e => e.ServerMembers).HasForeignKey(cm => cm.ServerId));

        // one Server -> many Channels
        builder.HasMany(s => s.Channels)
            .WithOne(c => c.Server)
            .HasForeignKey(c => c.ServerId);
    }
}
