using System.Collections;
using System.Text.Json;
using Auth.Domain.Aggregates;
using StackExchange.Redis;

namespace Auth.Infrastructure.Redis;

public class RedisCache : ICache,IRedisCache, ICacheLinkList, ICacheLocker
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
    public RedisCache(ConnectionMultiplexer? redisConn)
    {
        _redisConn = redisConn ?? throw new ArgumentNullException(nameof(redisConn));
        _aggregates = new List<AggregateRoot>();
    }

    /// <summary>
    /// 取得 Aggregate 裏面所有的 Domain Event
    /// </summary>
    /// <returns></returns>
    public List<AggregateRoot> GetAggregates()
    {
        return _aggregates;
    } 
        
    
    /// <summary>
    /// 鍵值是否存在
    /// </summary>
    /// <param name="hashKey">Redis Key</param>
    /// <returns></returns>
    public async Task<bool> IsKeyExist(string hashKey)
    {
        IDatabase db = _redisConn.GetDatabase();
        return await db.KeyExistsAsync(hashKey);
    }

    /// <summary>
    /// 寫入Cache 有時效性
    /// </summary>
    /// <param name="hashKey">Redis Key</param>
    /// <param name="hour">小時</param>
    /// <param name="minute">分</param>
    /// <param name="second">秒</param>
    /// <param name="json">Json資料</param>
    /// <returns></returns>
    public async Task<bool> SetExpiredKey(string hashKey, int hour, int minute, int second, string json)
    {
        IDatabase db = _redisConn.GetDatabase();
        return await db.StringSetAsync(
                            key: hashKey,
                            value: json,
                            expiry: new TimeSpan(hour, minute, second));
    }

    /// <summary>
    /// 取得Cache內容
    /// </summary>
    /// <typeparam name="T">轉換的物件</typeparam>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    public async Task<T> GetObjectAsync<T>(string key)
    {
        var db = _redisConn.GetDatabase();
        var value = await db.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return default;

        return JsonSerializer.Deserialize<T>(value);
    }

    /// <summary>
    /// 取得Cache內容
    /// </summary>
    /// <typeparam name="T">轉換的物件</typeparam>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    public T GetObject<T>(string key)
    {
        var db = _redisConn.GetDatabase();
        var value = db.StringGet(key);
        if (value.IsNullOrEmpty)
            return default;
        return JsonSerializer.Deserialize<T>(db.StringGet(key));
    }

    /// <summary>
    /// 刪除資料
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    public async Task<bool> RemoveCacheAsync(string key)
    {
        var db = _redisConn.GetDatabase();
        return await db.KeyDeleteAsync(key);
    }

    /// <summary>
    /// 刪除資料
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    public bool RemoveCache(string key)
    {
        var db = _redisConn.GetDatabase();
        return db.KeyDelete(key);
    }

    /// <summary>
    /// 寫入Cache 有時效性
    /// </summary>
    /// <typeparam name="T">轉換的物件</typeparam>
    /// <param name="key">Redis Key</param>
    /// <param name="obj">轉換的物件</param>
    /// <param name="expiry">多久過期</param>
    /// <returns></returns>
    public async Task<bool> SetCacheAsync<T>(string key, T obj, TimeSpan expiry = default)
    {
        try
        {
            if (expiry == default)
                expiry = new TimeSpan(0, 8, 0);
            var db = _redisConn.GetDatabase();
            var json = JsonSerializer.Serialize(obj);
            return await db.StringSetAsync(key: key, value: json, expiry: expiry);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    /// <summary>
    /// 寫入Cache 有時效性
    /// </summary>
    /// <typeparam name="T">轉換的物件</typeparam>
    /// <param name="key">Redis Key</param>
    /// <param name="obj">轉換的物件</param>
    /// <param name="expiry">多久過期</param>
    /// <returns></returns>
    public bool SetCache<T>(string key, T obj, TimeSpan expiry = default)
    {
        if (expiry == default)
            expiry = new TimeSpan(0, 8, 0);
        var db = _redisConn.GetDatabase();
        return db.StringSet(key: key, value: JsonSerializer.Serialize(obj), expiry: expiry);
    }

    /// <summary>
    /// LPUSH List 加入列表內
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="objs">要加入之列表</param>
    /// <returns></returns>
    public long LPushList(string key, IEnumerable<string> objs)
    {
        var redisValues = Array.ConvertAll(objs.ToArray(), item => (RedisValue)item);
        var db = _redisConn.GetDatabase();
        return db.ListLeftPush(key, redisValues);
    }

    /// <summary>
    /// LPUSH Async List 加入列表內
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="objs">要加入之列表</param>
    /// <returns></returns>
    public async Task<long> LPushListAsync(string key, IEnumerable<string> objs)
    {
        var redisValues = Array.ConvertAll(objs.ToArray(), item => (RedisValue)item);
        var db = _redisConn.GetDatabase();
        return await db.ListLeftPushAsync(key, redisValues);
    }

    /// <summary>
    /// LPUSH String 加入列表內
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="str">要推送一筆的內容</param>
    /// <returns></returns>
    public long LPush(string key, string str)
    {
        var db = _redisConn.GetDatabase();
        return db.ListLeftPush(key, str);
    }

    /// <summary>
    /// LPUSH Async String 加入列表內
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="str">要推送一筆的內容</param>
    /// <returns></returns>
    public async Task<long> LPushAsync(string key, string str)
    {
        var db = _redisConn.GetDatabase();
        return await db.ListLeftPushAsync(key, str);
    }

    /// <summary>
    /// LPOP String 取出一筆
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    public string LPop(string key)
    {
        var db = _redisConn.GetDatabase();
        return db.ListLeftPop(key);
    }

    /// <summary>
    /// LPOP Async String 取出一筆
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    public async Task<string> LPopAsync(string key)
    {
        var db = _redisConn.GetDatabase();
        return await db.ListLeftPopAsync(key);
    }

    /// <summary>
    /// LRange Length 取得長度
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    public async Task<long> LLengthAsync(string key)
    {
        var db = _redisConn.GetDatabase();
        return await db.ListLengthAsync(key);
    }

    /// <summary>
    /// LRange Length 取得長度
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    public long LLength(string key)
    {
        var db = _redisConn.GetDatabase();
        return db.ListLength(key);
    }

    /// <summary>
    /// Lock Redis資料
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="value">列表裡面裡面其中一個值</param>
    /// <returns></returns>
    public async Task<bool> LockAsync(string key, string value)
    {
        var db = _redisConn.GetDatabase();
        return await db.LockTakeAsync(key, value, new TimeSpan(0, 0, 30));
    }

    /// <summary>
    /// 解除Lock Redis資料
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    public async Task<bool> UnlockAsync(string key)
    {
        var db = _redisConn.GetDatabase();
        return await db.KeyDeleteAsync(key);
    }

    /// <summary>
    /// 設定Redis資料過期時間
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="expiredTime">過期時間</param>
    /// <returns></returns>
    public async Task<bool> SetKeyExpiredAsync(string key, DateTime? expiredTime)
    {
        var db = _redisConn.GetDatabase();
        if (expiredTime == null)
            expiredTime = DateTime.UtcNow.AddMinutes(8);
        return await db.KeyExpireAsync(key, expiredTime);
    }

    /// <summary>
    /// 設定Redis資料過期時間
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="expiredTime">過期時間</param>
    /// <returns></returns>
    public async Task<bool> SetKeyExpiredAsync(string key, TimeSpan? expiredTime)
    {
        var db = _redisConn.GetDatabase();
        if (expiredTime == null)
            expiredTime = new TimeSpan(0, 8, 0);
        return await db.KeyExpireAsync(key, expiredTime);
    }

    /// <summary>
    /// 設定Redis資料過期時間
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="expiredTime">過期時間</param>
    /// <returns></returns>
    public bool SetKeyExpired(string key, DateTime? expiredTime)
    {
        var db = _redisConn.GetDatabase();
        if (expiredTime == null)
            expiredTime = DateTime.UtcNow.AddMinutes(8);
        return db.KeyExpire(key, expiredTime);
    }

    /// <summary>
    /// 設定Redis資料過期時間
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="expiredTime">過期時間</param>
    /// <returns></returns>
    public bool SetKeyExpired(string key, TimeSpan? expiredTime)
    {
        var db = _redisConn.GetDatabase();
        if (expiredTime == null)
            expiredTime = new TimeSpan(0, 8, 0);
        return db.KeyExpire(key, expiredTime);
    }

    #region 關於 Hash Set 的部分
    
    /// <summary>
    /// 設定 Hash雜湊對應
    /// </summary>
    /// <param name="key">Redis紀錄Key</param>
    /// <param name="field">Redis此記錄Key底下的Identity</param>
    /// <param name="json">儲存的物件</param>
    /// <param name="expireSeconds">過期日子</param>
    public async Task SetHashCacheAsync(string key,string field,string json,int expireSeconds = 300)
    {
        var db = _redisConn.GetDatabase();
        var hashSet = db.HashSetAsync(key, field, json);
        var expireSet = db.KeyExpireAsync(key, new TimeSpan(0, 0, expireSeconds));
        await hashSet;
        await expireSet;

    }

    /// <summary>
    /// 設定 Hash雜湊對應
    /// </summary>
    /// <param name="key">Redis紀錄Key</param>
    /// <param name="field">Redis此記錄Key底下的Identity</param>
    /// <param name="json">儲存的物件</param>
    /// <param name="expireSeconds">過期日子</param>
    public async Task SetHashCacheAsync<T>(string key, string field, T json, int expireSeconds = 300)
    {
        var db = _redisConn.GetDatabase();
        var hashSet = db.HashSetAsync(key, field, JsonSerializer.Serialize(json));
        var expireSet = db.KeyExpireAsync(key, new TimeSpan(0, 0, expireSeconds));
        await hashSet;
        await expireSet;

    }

    /// <summary>
    /// 取得 Hash雜湊物件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">Redis紀錄Key</param>
    /// <param name="field">Redis此記錄Key底下的Identity</param>
    /// <returns></returns>
    public async Task<T> GetHashCacheAsync<T>(string key,string field)
    {
        var db = _redisConn.GetDatabase();
        var hashSet = await db.HashGetAsync(key, field);
        if(hashSet.IsNullOrEmpty)
        {
            return default(T);
        }
        return JsonSerializer.Deserialize<T>(hashSet);
    }

    /// <summary>
    /// 從 HashSet 中重組 Object
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> GetHashCacheAsync<T>(string key) where T : class, new()
    {
        var db = _redisConn.GetDatabase();
        var hashEntries = await db.HashGetAllAsync(key);
        if (!hashEntries.Any()) return null;
        var obj = HashEntryArrayToObject<T>(hashEntries);
        return obj;
    }
    
    /// <summary>
    /// Object 儲存成 HashSet
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    /// <param name="expireSeconds"></param>
    /// <typeparam name="T"></typeparam>
    public async Task SetHashCacheAsync<T>(string key, T obj, int expireSeconds = 300) where T : AggregateRoot
    {
        var db = _redisConn.GetDatabase();
        await db.HashSetAsync(key, ConvertToHashEntryArray(obj));
        await db.KeyExpireAsync(key, new TimeSpan(0, 0, expireSeconds));
        _aggregates.Add(obj);
    }

    /// <summary>
    /// HashEntries 轉成 Object
    /// </summary>
    /// <param name="hashEntries"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static T HashEntryArrayToObject<T>(HashEntry[] hashEntries) where T : new()
    {
        var data = new T();
        foreach (var property in typeof(T).GetProperties())
        {
            var isEnumerable = property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType);

            if (!isEnumerable)
            {
                var value = hashEntries.First(entry => entry.Name == property.Name).Value.ToString();
                var type =  property.PropertyType;

                if (type == typeof(string))
                    property.SetValue(data,value,null);
                else if (type == typeof(char))
                    property.SetValue(data,char.Parse(value),null);
                else if(type == typeof(decimal))
                    property.SetValue(data, decimal.Parse(value),null);
                else if(type == typeof(double))
                    property.SetValue(data, double.Parse(value),null);
                else if(type == typeof(float))
                    property.SetValue(data, float.Parse(value),null);
                else if(type == typeof(long))
                    property.SetValue(data, long.Parse(value),null);
                else if(type == typeof(int))
                    property.SetValue(data, int.Parse(value),null);
                else if(type == typeof(bool))
                    property.SetValue(data, bool.Parse(value),null);
            }
        }
        return data;
    }
    
    /// <summary>
    /// Object 轉換成 HashEntryList 方便儲存到 Hash Set 當中
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    private static HashEntry[] ConvertToHashEntryArray(object instance)
    {
        var properties = new List<HashEntry>();
        foreach (var property in instance.GetType().GetProperties())
        {
            var isEnumerable = property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType);

            if (!isEnumerable)
            {
                properties.Add(new HashEntry(property.Name,property.GetValue(instance).ToString()));
            }
        }
        return properties.ToArray();
    } 
    
    #endregion
    

    /// <summary>
    /// 排行資料增加分數
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">唯一值</param>
    /// <param name="addNum">如果存在唯一值則+1，不存在則建立增加並變成1</param>
    /// <returns></returns>
    public async Task SortedSetIncrementAsync(string key,string value,double addNum)
    {
        var db = _redisConn.GetDatabase();
        await db.SortedSetIncrementAsync(key, value, addNum);
    }

    /// <summary>
    /// 排行資料減少分數
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">唯一值</param>
    /// <param name="addNum">如果存在唯一值則-1，不存在則建立增加並變成-1</param>
    /// <returns></returns>
    public async Task SortedSetDecrementAsync(string key, string value, double addNum)
    {
        var db = _redisConn.GetDatabase();
        await db.SortedSetDecrementAsync(key, value, addNum);
    }

    /// <summary>
    /// 取得排行資料其中一位資料
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">唯一值</param>
    /// <returns></returns>
    public async Task<double?> GetSortedSetScoreAsync(string key,string value)
    {
        var db = _redisConn.GetDatabase();
        return await db.SortedSetScoreAsync(key, value);
    }
}