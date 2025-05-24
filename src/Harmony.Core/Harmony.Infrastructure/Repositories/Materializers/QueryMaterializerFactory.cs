using System.Linq.Expressions;
using Harmony.Application.Common.Interfaces;

namespace Harmony.Infrastructure.Repositories.Materializers;

public class QueryMaterializerFactory : IQueryMaterializerFactory
{
    public IQueryMaterializer<TResult, bool> AllAsync<TResult>(Expression<Func<TResult, bool>> predicate)
    {
        return new AllMaterializer<TResult>(predicate);
    }

    public IQueryMaterializer<TResult, bool> AnyAsync<TResult>()
    {
        return new AnyMaterializer<TResult>();
    }

    public IQueryMaterializer<TResult, int> CountAsync<TResult>()
    {
        return new CountMaterializer<TResult>();
    }

    public IQueryMaterializer<TResult, TResult> FirstAsync<TResult>()
    {
        return new FirstMaterializer<TResult>();
    }

    public IQueryMaterializer<TResult, TResult?> FirstOrDefaultAsync<TResult>()
    {
        return new FirstOrDefaultMaterializer<TResult>();
    }

    public IQueryMaterializer<TResult, long> LongCountAsync<TResult>()
    {
        return new LongCountMaterializer<TResult>();
    }

    public IQueryMaterializer<TResult, TResult> SingleAsync<TResult>()
    {
        return new SingleMaterializer<TResult>();
    }

    public IQueryMaterializer<TResult, TResult?> SingleOrDefaultAsync<TResult>()
    {
       return new SingleOrDefaultMaterializer<TResult>();
    }

    public IQueryMaterializer<TResult, TResult[]> ToArrayAsync<TResult>()
    {
        return new ToArrayMaterializer<TResult>();
    }

    public IQueryMaterializer<TResult, Dictionary<TKey, TResult>> ToDictionaryAsync<TKey, TResult>(Func<TResult, TKey> keySelector) where TKey : notnull
    {
        return new ToDictionaryMaterializer<TResult, TKey>(keySelector);
    }

    public IQueryMaterializer<TResult, List<TResult>> ToListAsync<TResult>()
    {
        return new ToListMaterializer<TResult>();
    }
}
