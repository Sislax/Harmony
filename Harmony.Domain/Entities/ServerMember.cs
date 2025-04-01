using Harmony.Domain.Enums;

namespace Harmony.Domain.Entities;

public class ServerMember
{
    public required Guid ServerId { get; set; }
    public required string UserId { get; set; }
    public required UserRole UserRole { get; set; }

    /// <summary>
    /// Navigation Property One to Many
    /// </summary>
    public Server Server { get; set; } = null!;

    /// <summary>
    /// Navigation Property One to Many
    /// </summary>
    public User User { get; set; } = null!;
}
