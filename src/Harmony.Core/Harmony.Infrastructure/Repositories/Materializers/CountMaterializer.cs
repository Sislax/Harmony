using Harmony.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Infrastructure.Repositories.Materializers;

public class CountMaterializer<TResult> : IQueryMaterializer<TResult, int>
{
    public async Task<int> MaterializeAsync(IQueryable<TResult> query, CancellationToken cancellationToken = default)
    {
        return await query.CountAsync(cancellationToken);
    }
}
