using Harmony.Domain.Common;

namespace Harmony.Domain.Entities;

public class Message : BaseEntity<Guid>
{
    public Guid ChannelId { get; set; }
    public required string MessageContent { get; set; }
    public required string UserId { get; set; }
    public DateTime SentAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Navigation Property One To Many
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Navigation Property One To Many
    /// </summary>
    public Channel Channel { get; set; } = null!;
}
