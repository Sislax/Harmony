using Harmony.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Infrastructure.Repositories.Materializers;

public class LongCountMaterializer<TResult> : IQueryMaterializer<TResult, long>
{
    public async Task<long> MaterializeAsync(IQueryable<TResult> query, CancellationToken cancellationToken = default)
    {
        return await query.LongCountAsync(cancellationToken);
    }
}
