using Harmony.Domain.Entities;

namespace Harmony.Application.Models.AuthResponseModels;

public class RefreshTokenResponseModel
{
    public bool IsSucceded { get; set; }

    /// <summary>
    /// RefreshToken is nullable because it is not guaranteed that the response will be successful
    /// </summary>
    public RefreshToken? RefreshToken { get; set; }
}
