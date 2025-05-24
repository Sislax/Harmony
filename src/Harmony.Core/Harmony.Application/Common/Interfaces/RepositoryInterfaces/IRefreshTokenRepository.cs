using Harmony.Domain.Entities;

namespace Harmony.Application.Common.Interfaces.RepositoryInterfaces;

public interface IRefreshTokenRepository
{
    Task<List<RefreshToken>?> GetAllUserRefreshToken(string userId);

    Task<RefreshToken?> GetRefreshToken(string refreshToken);

    void InsertRefreshToken(RefreshToken refreshToken);

    void UpdateRange(IEnumerable<RefreshToken> refreshTokens);

    void UpdateRefreshToken(RefreshToken refreshToken);

    void RemoveRange(IEnumerable<RefreshToken> refreshTokens);
}
