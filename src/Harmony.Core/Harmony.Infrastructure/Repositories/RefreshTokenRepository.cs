using Harmony.Application.Common.Interfaces.RepositoryInterfaces;
using Harmony.Domain.Entities;
using Harmony.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Infrastructure.Repositories;

public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
    }
}
