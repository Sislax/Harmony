using Harmony.Application.Models.DTOs;
using Harmony.Domain.Entities;

namespace Harmony.Application.Common.Interfaces;

public interface ITokenGenerator
{
    string GenerateJwtToken(UserForTokenDTO user);
    RefreshToken GenerateRefreshToken(UserForTokenDTO user);
    RefreshToken ExtendRefreshTokenExpiration(RefreshToken refreshToken);
}
