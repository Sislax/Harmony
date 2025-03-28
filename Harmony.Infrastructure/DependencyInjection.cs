using System.Text;
using Harmony.Application.Common.Interfaces;
using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using Harmony.Infrastructure.Data;
using Harmony.Infrastructure.Models.Identity;
using Harmony.Infrastructure.Models.Security;
using Harmony.Infrastructure.Repositories;
using Harmony.Infrastructure.Security;
using Harmony.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Harmony.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureServices();

        services.AddIdentity();

        services.AddAuthentication(configuration);

        services.AddPersistance(configuration);

        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));

        services.AddSingleton<ITokenGenerator, TokenGenerator>();

        services.ConfigureOptions<JwtTokenValidationConfiguration>()
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
            });

        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            //Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            //Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 4;

            //SignIn settings
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            //User settings
            options.User.RequireUniqueEmail = true;
        });

        services.AddScoped<IIdentityService, IdentityServices>();

        return services;
    }

    public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            )
        );

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static async Task AddSeedData(this IServiceProvider serviceProvider)
    {
        using (UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>())
        {
            using (RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>())
            {
                // Create Admin role if it doesn't exist
                IdentityRole? adminRole = await roleManager.FindByNameAsync("Admin");

                if (adminRole == null)
                {
                    _ = await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                // Create RegularUser role if it doesn't exist
                IdentityRole? regularUserRole = await roleManager.FindByNameAsync("RegularUser");

                if (regularUserRole == null)
                {
                    _ = await roleManager.CreateAsync(new IdentityRole("RegularUser"));
                }

                PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();

                // Create Admin user if it doesn't exist
                ApplicationUser? adminUser = await userManager.FindByEmailAsync("Admin@test.com");

                IUserRepository userRepository = serviceProvider.GetRequiredService<IUserRepository>();

                if (adminUser == null)
                {
                    ApplicationUser newAdminUser = new ApplicationUser()
                    {
                        FirstName = "Admin",
                        LastName = "Test",
                        Email = "Admin@test.com",
                        UserName = "AdminUser",
                    };

                    newAdminUser.PasswordHash = hasher.HashPassword(newAdminUser, "11&&MMxx");

                    _ = await userManager.CreateAsync(newAdminUser);

                    // Add Admin role to Admin user
                    IdentityRole? existingAdminRole = await roleManager.FindByNameAsync("Admin");

                    if (existingAdminRole != null)
                    {
                        string? adminRoleName = existingAdminRole.Name;

                        if (adminRoleName != null)
                        {
                            _ = await userManager.AddToRoleAsync(newAdminUser, adminRoleName);
                        }
                    }

                    // Create Domain User entity for Admin user
                    userRepository.InsertUser(new User()
                    {
                        Id = newAdminUser.Id,
                        FirstName = newAdminUser.FirstName,
                        LastName = newAdminUser.LastName,
                        Email = newAdminUser.Email,
                        Username = newAdminUser.UserName
                    });
                }

                // Create RegularUser user if it doesn't exist
                ApplicationUser? regularUser = await userManager.FindByEmailAsync("RegularUser@test.com");

                if (regularUser == null)
                {
                    ApplicationUser newRegularUser = new ApplicationUser()
                    {
                        FirstName = "RegularUser",
                        LastName = "Test",
                        Email = "RegularUser@test.com",
                        UserName = "RegularUser"
                    };

                    newRegularUser.PasswordHash = hasher.HashPassword(newRegularUser, "00!!LLpp");

                    _ = await userManager.CreateAsync(newRegularUser);

                    // Add RegularUser role to RegularUser user
                    IdentityRole? existingRegularUserRole = await roleManager.FindByNameAsync("RegularUser");

                    if (existingRegularUserRole != null)
                    {
                        string? adminRoleName = existingRegularUserRole.Name;

                        if (adminRoleName != null)
                        {
                            _ = await userManager.AddToRoleAsync(newRegularUser, adminRoleName);
                        }
                    }

                    // Create Domain User entity for RegularUser user
                    userRepository.InsertUser(new User()
                    {
                        Id = newRegularUser.Id,
                        FirstName = newRegularUser.FirstName,
                        LastName = newRegularUser.LastName,
                        Email = newRegularUser.Email,
                        Username = newRegularUser.UserName
                    });
                }

                IUnitOfWork unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}
