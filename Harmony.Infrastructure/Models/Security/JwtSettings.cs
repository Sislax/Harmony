namespace Harmony.Infrastructure.Models.Security;

public class JwtSettings
{
    public const string Section = "JwtSettings";
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string SecretKey { get; set; }
    public int TokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
}
