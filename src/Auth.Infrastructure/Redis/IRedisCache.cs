using Auth.Domain.Aggregates;

namespace Auth.Infrastructure.Redis;

public interface IRedisCache
{
    /// <summary>
    /// 取得 Aggregate 裏面所有的 Domain Event
    /// </summary>
    /// <returns></returns>
    public List<AggregateRoot> GetAggregates();
    
    /// <summary>
    /// 設定 Hash雜湊對應
    /// </summary>
    /// <param name="key">Redis紀錄Key</param>
    /// <param name="field">Redis此記錄Key底下的Identity</param>
    /// <param name="json">儲存的物件</param>
    /// <param name="expireSeconds">過期日子</param>
    Task SetHashCacheAsync(string key, string field, string json, int expireSeconds = 1);

    /// <summary>
    /// 設定 Hash雜湊對應
    /// </summary>
    /// <param name="key">Redis紀錄Key</param>
    /// <param name="field">Redis此記錄Key底下的Identity</param>
    /// <param name="json">儲存的物件</param>
    /// <param name="expireSeconds">過期日子</param>
    Task SetHashCacheAsync<T>(string key, string field, T json, int expireSeconds = 1);

    /// <summary>
    /// 取得 Hash雜湊物件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">Redis紀錄Key</param>
    /// <param name="field">Redis此記錄Key底下的Identity</param>
    /// <returns></returns>
    Task<T> GetHashCacheAsync<T>(string key, string field);

    /// <summary>
    /// 從 HashSet 中重組 Object
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> GetHashCacheAsync<T>(string key) where T : class, new();

    /// <summary>
    /// Object 儲存成 HashSet
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    /// <param name="expireSeconds"></param>
    /// <typeparam name="T"></typeparam>
    Task SetHashCacheAsync<T>(string key, T obj, int expireSeconds = 300) where T : AggregateRoot;

    /// <summary>
    /// 排行資料增加分數
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">唯一值</param>
    /// <param name="addNum">如果存在唯一值則+1，不存在則建立增加並變成1</param>
    /// <returns></returns>
    Task SortedSetIncrementAsync(string key, string value, double addNum);

    /// <summary>
    /// 排行資料減少分數
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">唯一值</param>
    /// <param name="addNum">如果存在唯一值則-1，不存在則建立增加並變成-1</param>
    /// <returns></returns>
    Task SortedSetDecrementAsync(string key, string value, double addNum);

    /// <summary>
    /// 取得排行資料其中一位資料
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">唯一值</param>
    /// <returns></returns>
    Task<double?> GetSortedSetScoreAsync(string key, string value);

    /// <summary>
    /// 刪除資料
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    Task<bool> RemoveCacheAsync(string key);
}