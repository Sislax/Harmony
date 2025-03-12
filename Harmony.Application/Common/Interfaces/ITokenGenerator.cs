using Harmony.Application.Models.DTOs;
using Harmony.Domain.Entities;

namespace Harmony.Application.Common.Interfaces;

public interface ITokenGenerator
{
    TokensDTO GenerateTokensAsync(UserForTokenDTO user);
    RefreshToken GenerateRefreshToken(UserForTokenDTO user);
}
