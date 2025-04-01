﻿using Harmony.Domain.Common;
namespace Harmony.Domain.Entities;

/// <summary>
/// Represents a user entity in the Domain layer.
/// It is different from the ApplicationUser class in the Infrastructure layer which is used for authentication and authorization.
/// </summary>
public class User : BaseEntity<string>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Navigation Property One to Many
    /// </summary>
    public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    /// <summary>
    /// Skip Navigation Property Many to Many
    /// </summary>
    public List<Server> Servers { get; set; } = new List<Server>();

    /// <summary>
    /// Navigation Property One to Many
    /// </summary>
    public List<ServerMember> ServerMembers { get; set; } = new List<ServerMember>();

    /// <summary>
    /// Skip Navigation Property Many to Many
    /// </summary>
    public List<Channel> Channels { get; set; } = new List<Channel>();

    /// <summary>
    /// Navigation Property One to Many
    /// </summary>
    public List<ChannelMember> ChannelMembers { get; set; } = new List<ChannelMember>();

    /// <summary>
    /// Navigation Property One to Many
    /// </summary>
    public List<Message> Messages { get; set; } = new List<Message>();
}
