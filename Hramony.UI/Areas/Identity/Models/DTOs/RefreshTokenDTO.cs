namespace Hramony.UI.Areas.Identity.Models.DTOs
{
    public class RefreshTokenDTO
    {
        public required string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public required string UserId { get; set; }
    }
}
