using System.Linq.Expressions;
using Harmony.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Infrastructure.Repositories.Materializers;

public class AllMaterializer<TResult> : IQueryMaterializer<TResult, bool>
{
    private readonly Expression<Func<TResult, bool>> _predicate;

    public AllMaterializer(Expression<Func<TResult, bool>> predicate)
    {
        _predicate = predicate;
    }

    public async Task<bool> MaterializeAsync(IQueryable<TResult> query, CancellationToken cancellationToken = default)
    {
        return await query.AllAsync(_predicate, cancellationToken);
    }
}
