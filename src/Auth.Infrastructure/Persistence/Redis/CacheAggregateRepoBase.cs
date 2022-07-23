using Auth.Domain.Aggregates;
using StackExchange.Redis;

namespace Auth.Infrastructure.Persistence.Redis;

public abstract class CacheAggregateRepoBase<T> where T : AggregateRoot
{
    /// <summary>
    /// 把 HashEntry[] 轉成 Aggregate
    /// </summary>
    /// <param name="entries"></param>
    /// <returns></returns>
    protected abstract T HashEntriesToObject(HashEntry[] entries);

    /// <summary>
    /// 把 Aggregate 轉成 HashEntry[]
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected abstract HashEntry[] GetHashEntries(T entity);
}