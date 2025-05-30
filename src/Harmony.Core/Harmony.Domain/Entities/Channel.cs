using Harmony.Domain.Common;
using Harmony.Domain.Enums;

namespace Harmony.Domain.Entities;

public class Channel : BaseEntity<Guid>
{
    public Guid ServerId { get; set; }
    public required string ChannelName { get; set; }
    public required ChannelType ChannelType { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Navigation Property One to One
    /// </summary>
    public Server Server { get; set; } = null!;

    /// <summary>
    /// Skip Navigation Property Many to Many
    /// </summary>
    public List<User> Users { get; set; } = [];

    /// <summary>
    /// Navigation Property One to Many
    /// </summary>
    public List<ChannelMember> ChannelMembers { get; set; } = [];

    /// <summary>
    /// Navigation Property One to Many
    /// </summary>
    public List<Message> Messages { get; set; } = [];
}
