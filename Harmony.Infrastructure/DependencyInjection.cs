using Harmony.Application.Common.Interfaces;
using Harmony.Infrastructure.Data;
using Harmony.Infrastructure.Models.Identity;
using Harmony.Infrastructure.Models.Security;
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
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
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

    public static async Task AddSeedData(this IServiceProvider serviceProvider)
    {
        using (RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>())
        {
            if (roleManager != null)
            {
                IdentityRole? adminRole = await roleManager.FindByNameAsync("Admin");

                if (adminRole == null)
                {
                    _ = await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                IdentityRole? regularUserRole = await roleManager.FindByNameAsync("RegularUSer");

                if (regularUserRole == null)
                {
                    _ = await roleManager.CreateAsync(new IdentityRole("RegularUSer"));
                }
            }
        }

        using (UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>())
        {
            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();

            if (userManager != null)
            {
                ApplicationUser? adminUser = await userManager.FindByEmailAsync("Admin@test.com");

                if (adminUser == null)
                {
                    ApplicationUser newAdminUser = new ApplicationUser()
                    {
                        FirstName = "Admin",
                        LastName = "Test",
                        Email = "Admin@test.com",
                        UserName = "AdminUser"
                    };

                    hasher.HashPassword(newAdminUser, "11&&MMxx");

                    _ = await userManager.CreateAsync(newAdminUser);
                }

                ApplicationUser? regularUser = await userManager.FindByEmailAsync("RegularUser@test.com");

                if (regularUser == null)
                {
                    ApplicationUser newRegularUser = new ApplicationUser()
                    {
                        FirstName = "Admin",
                        LastName = "Test",
                        Email = "RegularUser@test.com",
                        UserName = "RegularUser"
                    };

                    hasher.HashPassword(newRegularUser, "00!!LLpp");

                    _ = await userManager.CreateAsync(newRegularUser);
                }
            }
        }
    }
}
