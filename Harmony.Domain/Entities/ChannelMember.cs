namespace Harmony.Domain.Entities;

/// <summary>
/// This class is used to manage the members of a channel and also direct messages
/// </summary>
public class ChannelMember
{
    public required string UserId { get; set; }
    public required Guid ChannelId { get; set; }

    /// <summary>
    /// Navigation Property One to Many
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Navigation Property One to Many
    /// </summary>
    public Channel Channel { get; set; } = null!;
}
