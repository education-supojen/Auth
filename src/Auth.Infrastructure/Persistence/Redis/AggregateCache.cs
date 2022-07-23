using System.Text.Json;
using Auth.Domain.Aggregates;
using StackExchange.Redis;

namespace Auth.Infrastructure.Persistence.Redis;

public class AggregateCache : IAggregateCache
{
    /// <summary>
    /// ConnectionMultiplexer
    /// </summary>
    private readonly ConnectionMultiplexer _redisConn;

    /// <summary>
    /// Aggregates
    /// </summary>
    private readonly List<AggregateRoot> _aggregates;
    

    /// <summary>
    /// RedisCache
    /// </summary>
    /// <param name="redisConn"></param>
    public AggregateCache(ConnectionMultiplexer? redisConn)
    {
        _redisConn = redisConn ?? throw new ArgumentNullException(nameof(redisConn));
        _aggregates = new List<AggregateRoot>();
    }

    /// <summary>
    /// 取得 Aggregate 裏面所有的 Domain Event
    /// </summary>
    /// <returns></returns>
    public List<AggregateRoot> GetAggregates() => _aggregates;


    /// <summary>
    /// Redis Hash Set 存處 Aggregate
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    /// <param name="func"></param>
    /// <param name="expireSeconds"></param>
    /// <typeparam name="T"></typeparam>
    public async Task SaveAsync<T>(string key, T obj, Func<T, HashEntry[]> func, int expireSeconds = 300) where T : AggregateRoot
    {
        // Processing - 取得連線
        var db = _redisConn.GetDatabase();
        // Processing - Aggregate 存入 Redis 當中(Hash Set 結構)
        await db.HashSetAsync(key, func(obj));
        // Processing - 設定過期時間
        await db.KeyExpireAsync(key, new TimeSpan(0, 0, expireSeconds));
        // Processing - 處存 Aggregate
        _aggregates.Add(obj);
    }

    /// <summary>
    /// 取得 Hash雜湊物件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">Redis Hash Key</param>
    /// <returns></returns>
    public async Task<HashEntry[]> GetHashCacheAsync<T>(string key)
    {
        var db = _redisConn.GetDatabase();
        return await db.HashGetAllAsync(key);
    }

    /// <summary>
    /// 刪除資料
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<bool> DeleteAsync<T>(string key,T obj) where T : AggregateRoot
    {
        // Processing - Redis 連線
        var db = _redisConn.GetDatabase();
        // Processing - 刪除資料
        var success= await db.KeyDeleteAsync(key);
        // Processing - 紀錄 Aggregate
        if (success)
        {
            _aggregates.Add(obj);
        }
        // Processing - 沒有刪除成功, 返回 false
        return false;
    }
    
}