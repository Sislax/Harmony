namespace Harmony.API.Models
{
    public class Settings
    {
        public string JwtSecretKey { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public int JwtAccessTokenLifetimeMinutes { get; set; }

        public Settings(IConfiguration configuration)
        {
            JwtSecretKey = configuration.GetSection("Jwt").GetValue<string>("SecretKey") ?? throw new NullReferenceException();
            JwtIssuer = configuration.GetSection("Jwt").GetValue<string>("Issuer") ?? throw new NullReferenceException();
            JwtAudience = configuration.GetSection("Jwt").GetValue<string>("Audience") ?? throw new NullReferenceException();
            JwtAccessTokenLifetimeMinutes = configuration.GetSection("Jwt").GetValue<int>("AccessTokenLifetimeMinutes");
        }
    }
}
