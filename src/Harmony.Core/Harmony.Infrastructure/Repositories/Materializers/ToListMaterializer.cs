using Harmony.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Infrastructure.Repositories.Materializers;

public class ToListMaterializer<TResult> : IQueryMaterializer<TResult, List<TResult>>
{
    public async Task<List<TResult>> MaterializeAsync(IQueryable<TResult> query, CancellationToken cancellationToken = default)
    {
        return await query.ToListAsync(cancellationToken);
    }
}
