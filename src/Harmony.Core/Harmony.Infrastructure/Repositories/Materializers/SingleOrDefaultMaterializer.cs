using Harmony.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Infrastructure.Repositories.Materializers;

public class SingleOrDefaultMaterializer<TResult> : IQueryMaterializer<TResult, TResult?>
{
    public async Task<TResult?> MaterializeAsync(IQueryable<TResult> query, CancellationToken cancellationToken = default)
    {
        return await query.SingleOrDefaultAsync(cancellationToken);
    }
}