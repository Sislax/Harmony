using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using Harmony.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RefreshToken>?> GetAllUserRefreshToken(string userId)
    {
        return await _context.RefreshTokens.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<RefreshToken?> GetRefreshToken(string refreshToken)
    {
        return await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);
    }

    public void InsertRefreshToken(RefreshToken refreshToken)
    {
        _context.Add(refreshToken);
    }

    public void UpdateRange(List<RefreshToken> refreshTokens)
    {
        _context.UpdateRange(refreshTokens);
    }

    public void UpdateRefreshToken(RefreshToken refreshToken)
    {
        _context.Update(refreshToken);
    }
}
