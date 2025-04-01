using Harmony.Domain.Common;

namespace Harmony.Domain.Entities;

public class Server : BaseEntity<Guid>
{
    public required string ServerName { get; set; }
    public required string ServerOwnerId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Navigation Property one to many
    /// </summary>
    public User ServerOwner { get; set; } = null!;

    /// <summary>
    /// Skip Navigation Property many to many
    /// </summary>
    public List<User> Users { get; set; } = new List<User>();

    /// <summary>
    /// Navigation Property one to many
    /// </summary>
    public List<ServerMember> ServerMembers { get; set; } = new List<ServerMember>();

    /// <summary>
    /// Navigation Property one to many
    /// </summary>
    public List<Channel> Channels { get; set; } = new List<Channel>();

}
