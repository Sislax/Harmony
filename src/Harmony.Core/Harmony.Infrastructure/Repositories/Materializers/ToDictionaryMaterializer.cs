using Harmony.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Infrastructure.Repositories.Materializers;

public class ToDictionaryMaterializer<TResult, TKey> : IQueryMaterializer<TResult, Dictionary<TKey, TResult>> where TKey : notnull
{
    private readonly Func<TResult, TKey> _keySelector;

    public ToDictionaryMaterializer(Func<TResult, TKey> keySelector)
    {
        _keySelector = keySelector;
    }

    public async Task<Dictionary<TKey, TResult>> MaterializeAsync(IQueryable<TResult> query, CancellationToken cancellationToken = default)
    {
        return await query.ToDictionaryAsync(_keySelector, cancellationToken);
    }
}
