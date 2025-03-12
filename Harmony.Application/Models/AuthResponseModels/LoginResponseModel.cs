using Harmony.Domain.Entities;

namespace Harmony.Application.Models.AuthResponseModels;

public class LoginResponseModel
{
    public bool IsSucceded { get; set; }

    /// <summary>
    /// UserId is nullable because it is not guaranteed that the login process will be successful.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Token is nullable because it is not guaranteed that the login process will be successful.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// RefreshToken is nullable because it is not guaranteed that the login process will be successful.
    /// </summary>
    public RefreshToken? RefreshToken { get; set; }
}
