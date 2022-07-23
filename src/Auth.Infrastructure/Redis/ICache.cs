namespace Auth.Infrastructure.Redis;

public interface ICache
{
            /// <summary>
        /// 取得Cache內容
        /// </summary>
        /// <typeparam name="T">轉換的物件</typeparam>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        T GetObject<T>(string key);

        /// <summary>
        /// 取得Cache內容
        /// </summary>
        /// <typeparam name="T">轉換的物件</typeparam>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        Task<T> GetObjectAsync<T>(string key);

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        bool RemoveCache(string key);

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        Task<bool> RemoveCacheAsync(string key);

        /// <summary>
        /// 寫入Cache 有時效性
        /// </summary>
        /// <typeparam name="T">轉換的物件</typeparam>
        /// <param name="key">Redis Key</param>
        /// <param name="obj">轉換的物件</param>
        /// <param name="expiry">多久過期</param>
        /// <returns></returns>
        bool SetCache<T>(string key, T obj, TimeSpan expiry = default);

        /// <summary>
        /// 寫入Cache 有時效性
        /// </summary>
        /// <typeparam name="T">轉換的物件</typeparam>
        /// <param name="key">Redis Key</param>
        /// <param name="obj">轉換的物件</param>
        /// <param name="expiry">多久過期</param>
        /// <returns></returns>
        Task<bool> SetCacheAsync<T>(string key, T obj, TimeSpan expiry = default);

        /// <summary>
        /// 寫入Cache 有時效性
        /// </summary>
        /// <param name="hashKey">Redis Key</param>
        /// <param name="hour">小時</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        /// <param name="json">Json資料</param>
        /// <returns></returns>
        Task<bool> SetExpiredKey(string hashKey, int hour, int minute, int second, string json);

        /// <summary>
        /// 鍵值是否存在
        /// </summary>
        /// <param name="hashKey">Redis Key</param>
        /// <returns></returns>
        Task<bool> IsKeyExist(string hashKey);
}