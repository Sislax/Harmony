using Harmony.Domain.Entities;

namespace Harmony.Domain.Abstractions.RepositoryInterfaces;

public interface IRefreshTokenRepository
{
    void InsertRefreshToken(RefreshToken refreshToken);

    Task<List<RefreshToken>?> GetAllUserRefreshToken(string userId);

    Task<RefreshToken?> GetRefreshToken(string refreshToken);

    void UpdateRefreshToken(RefreshToken refreshToken);

    void UpdateRange(List<RefreshToken> refreshTokens);
}
