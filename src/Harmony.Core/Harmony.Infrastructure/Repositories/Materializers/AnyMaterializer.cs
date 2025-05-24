using Harmony.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Infrastructure.Repositories.Materializers;

public class AnyMaterializer<TResult> : IQueryMaterializer<TResult, bool>
{
    public async Task<bool> MaterializeAsync(IQueryable<TResult> query, CancellationToken cancellationToken = default)
    {
        return await query.AnyAsync(cancellationToken);
    }
}
