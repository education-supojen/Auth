using Auth.Domain.Aggregates;
using Auth.Domain.Repositories;
using Auth.Infrastructure.Persistence.Redis;

namespace Auth.Infrastructure.Persistence;

public class RegistrationRepository : IRegistrationRepository
{
    private const string REGISTRATION_KEY = "registration:";
    private readonly IRedisCache _redisCache;

    public RegistrationRepository(IRedisCache redisCache, ICache cache)
    { 
        _redisCache = redisCache;
    }

    /// <summary>
    /// 取得 - 註冊資料 (Email 是註冊資量的 Identity)
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<Registration> GetAsync(string email)
    {
        return await _redisCache.GetHashCacheAsync<Registration>(REGISTRATION_KEY + email);
    }

    /// <summary>
    /// 建立 - 註冊資料
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    public async Task AddAsync(Registration registration)
    {
        await _redisCache.SetHashCacheAsync(REGISTRATION_KEY + registration.Id, registration);
    }

    /// <summary>
    /// 更新 - 註冊資料
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    public async Task UpdateAsync(Registration registration)
    {
        await _redisCache.SetHashCacheAsync(REGISTRATION_KEY + registration.Id, registration);
    }

    /// <summary>
    /// 刪除 - 註冊資料
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    public async Task DeleteAsync(Registration registration)
    {
        await _redisCache.RemoveCacheAsync(REGISTRATION_KEY + registration.Id);
    }
}