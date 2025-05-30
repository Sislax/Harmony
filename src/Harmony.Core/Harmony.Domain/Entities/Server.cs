using Harmony.Domain.Common;

namespace Harmony.Domain.Entities;

public class Server : BaseEntity<Guid>
{
    public required string ServerName { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Skip Navigation Property many to many
    /// </summary>
    public List<User> Users { get; set; } = [];

    /// <summary>
    /// Navigation Property one to many
    /// </summary>
    public List<ServerMember> ServerMembers { get; set; } = [];

    /// <summary>
    /// Navigation Property one to many
    /// </summary>
    public List<Channel> Channels { get; set; } = [];

}
