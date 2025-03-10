using Harmony.Domain.Entities;

namespace Harmony.Application.Models.AuthResponseModels
{
    public class AuthResponseModel
    {
        public Guid UserId { get; set; }
        public required string Token { get; set; }
        public required RefreshToken RefreshToken { get; set; }
    }
}
