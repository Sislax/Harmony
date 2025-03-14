using Harmony.Domain.Common;
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
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Navigation Proprierty One to Many
    /// </summary>
    public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
