using Hramony.UI.Areas.Identity.Models.DTOs;

namespace Hramony.UI.Areas.Identity.Models.AuthResponseModel;

public class RefreshTokenResponseModel
{
    public bool IsSucceded { get; set; }

    /// <summary>
    /// RefreshToken is nullable because it is not guaranteed that the response will be successful
    /// </summary>
    public RefreshTokenDTO? RefreshToken { get; set; }
}
