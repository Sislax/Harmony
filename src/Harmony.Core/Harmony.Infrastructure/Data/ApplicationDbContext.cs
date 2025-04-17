using Harmony.Domain.Entities;
using Harmony.Infrastructure.Data.EnitityTypeConfigurations;
using Harmony.Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<User> DomainUsers { get; set; }
    public DbSet<Server> Servers { get; set; }
    public DbSet<ServerMember> ServerMembers { get; set; }
    public DbSet<Channel> Channels { get; set; }
    public DbSet<ChannelMember> ChannelMembers { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        new UserEntityTypeConfiguration().Configure(builder.Entity<User>());
        new ServerEntityTypeConfiguration().Configure(builder.Entity<Server>());
        new ServerMemberEntityTypeConfiguration().Configure(builder.Entity<ServerMember>());
        new ChannelEntityTypeConfiguration().Configure(builder.Entity<Channel>());
        new ChannelMemberEntityTypeConfiguration().Configure(builder.Entity<ChannelMember>());
        new MessageEntityTypeConfiguration().Configure(builder.Entity<Message>());
        new RefreshTokenEntityTypeConfiguration().Configure(builder.Entity<RefreshToken>());

        base.OnModelCreating(builder);
    }
}
