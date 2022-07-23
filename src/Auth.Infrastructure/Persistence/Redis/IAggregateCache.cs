using Auth.Domain.Aggregates;
using StackExchange.Redis;

namespace Auth.Infrastructure.Persistence.Redis;

public interface IAggregateCache
{
    /// <summary>
    /// 取得 Aggregate 裏面所有的 Domain Event
    /// </summary>
    /// <returns></returns>
    public List<AggregateRoot> GetAggregates();

    /// <summary>
    /// Redis Hash Set 存處 Aggregate
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    /// <param name="func"></param>
    /// <param name="expireSeconds"></param>
    /// <typeparam name="T"></typeparam>
    Task SaveAsync<T>(string key, T obj, Func<T, HashEntry[]> func, int expireSeconds = 300) where T : AggregateRoot;

    /// <summary>
    /// 取得 Hash雜湊物件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">Redis Hash Key</param>
    /// <returns></returns>
    Task<HashEntry[]> GetHashCacheAsync<T>(string key);


    /// <summary>
    /// 刪除資料
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<bool> DeleteAsync<T>(string key, T obj) where T : AggregateRoot;
    
}