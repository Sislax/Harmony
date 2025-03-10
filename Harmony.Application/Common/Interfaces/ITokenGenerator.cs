using Harmony.Application.Models.AuthResponseModels;
using Harmony.Application.Models.DTOs;

namespace Harmony.Application.Common.Interfaces
{
    public interface ITokenGenerator
    {
        TokensDTO GenerateTokensAsync(UserForTokenDTO user);
    }
}
