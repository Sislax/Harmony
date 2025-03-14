using System.Text;
using Harmony.Infrastructure.Models.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Harmony.Infrastructure.Security;

public sealed class JwtTokenValidationConfiguration : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenValidationConfiguration(IOptions<JwtSettings> jwtSettingsOptions)
    {
        _jwtSettings = jwtSettingsOptions.Value;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }

    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecretKey)),
            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }
}
