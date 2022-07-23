namespace Auth.Infrastructure.Redis;

public interface ICacheLocker
{
    /// <summary>
    /// Lock Redis資料
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <param name="value">列表裡面裡面其中一個值</param>
    /// <returns></returns>
    Task<bool> LockAsync(string key, string value);

    /// <summary>
    /// 解除Lock Redis資料
    /// </summary>
    /// <param name="key">Redis Key</param>
    /// <returns></returns>
    Task<bool> UnlockAsync(string key);
}