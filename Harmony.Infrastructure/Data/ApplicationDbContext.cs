using Harmony.Domain.Entities;
using Harmony.Infrastructure.Data.EnitityTypeConfigurations;
using Harmony.Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<User> DomainUsers { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        new UserEntityTypeConfiguration().Configure(builder.Entity<User>());
        new RefreshTokenEntityTypeConfiguration().Configure(builder.Entity<RefreshToken>());

        base.OnModelCreating(builder);
    }
}
