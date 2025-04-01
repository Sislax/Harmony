namespace Harmony.Application.Models.AuthResponseModels;

public class RefreshTokenResponseModel
{
    public bool IsSucceded { get; set; }

    /// <summary>
    /// Token is nullable because it is not guaranteed that the response will be successful
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// RefreshToken is nullable because it is not guaranteed that the response will be successful
    /// </summary>
    public string? RefreshToken { get; set; }
}
