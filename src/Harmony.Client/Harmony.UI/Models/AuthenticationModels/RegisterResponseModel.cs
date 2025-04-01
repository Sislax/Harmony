namespace Harmony.UI.Models.AuthenticationModels;

public class RegisterResponseModel
{
    public bool IsSucceded { get; set; }

    /// <summary>
    /// UserId is nullable because it is not guaranteed that the registration process will be successful.
    /// </summary>
    public string? UserId { get; set; }
}
