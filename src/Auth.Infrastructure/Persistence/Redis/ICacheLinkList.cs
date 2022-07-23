namespace Auth.Infrastructure.Persistence.Redis;

public interface ICacheLinkList
{
    /// <summary>
    /// LPUSH List 加入列表內
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="objs">要加入之列表</param>
    /// <returns></returns>
    long LPushList(string key, IEnumerable<string> objs);

    /// <summary>
    /// LPUSH Async List 加入列表內
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="objs">要加入之列表</param>
    /// <returns></returns>
    Task<long> LPushListAsync(string key, IEnumerable<string> objs);

    /// <summary>
    /// LPUSH String 加入列表內
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="str">要推送一筆的內容</param>
    /// <returns></returns>
    long LPush(string key, string str);

    /// <summary>
    /// LPUSH Async String 加入列表內
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="str">要推送一筆的內容</param>
    /// <returns></returns>
    Task<long> LPushAsync(string key, string str);

    /// <summary>
    /// LPOP String 取出一筆
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    string LPop(string key);

    /// <summary>
    /// LPOP Async String 取出一筆
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    Task<string> LPopAsync(string key);

    /// <summary>
    /// LRange Length 取得長度
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    Task<long> LLengthAsync(string key);

    /// <summary>
    /// LRange Length 取得長度
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    long LLength(string key);

    /// <summary>
    /// 設定Redis資料過期時間
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="expiredTime">過期時間</param>
    /// <returns></returns>
    Task<bool> SetKeyExpiredAsync(string key, DateTime? expiredTime);

    /// <summary>
    /// 設定Redis資料過期時間
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="expiredTime">過期時間</param>
    /// <returns></returns>
    Task<bool> SetKeyExpiredAsync(string key, TimeSpan? expiredTime);

    /// <summary>
    /// 設定Redis資料過期時間
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="expiredTime">過期時間</param>
    /// <returns></returns>
    bool SetKeyExpired(string key, DateTime? expiredTime);

    /// <summary>
    /// 設定Redis資料過期時間
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="expiredTime">過期時間</param>
    /// <returns></returns>
    bool SetKeyExpired(string key, TimeSpan? expiredTime);

    /// <summary>
    /// 移除快取
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    Task<bool> RemoveCacheAsync(string key);

    /// <summary>
    /// 移除快取
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    bool RemoveCache(string key);
}