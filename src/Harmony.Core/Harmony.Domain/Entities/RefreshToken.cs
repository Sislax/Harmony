using Harmony.Domain.Common;

namespace Harmony.Domain.Entities;

public class RefreshToken : BaseEntity<Guid>
{
    public required string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public required string UserId { get; set; }

    /// <summary>
    /// Navigation Property One to Many
    /// </summary>
    public User User { get; set; } = null!;
}
