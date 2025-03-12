using Harmony.Domain.Common;

namespace Harmony.Domain.Entities;

public class RefreshToken : BaseEntity<Guid>
{
    public required string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; } = false;
    public required string UserId { get; set; }

    public void Revoke()
    {
        IsRevoked = true;
    }
}
