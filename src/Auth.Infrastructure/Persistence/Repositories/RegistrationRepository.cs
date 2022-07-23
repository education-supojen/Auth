using System.Text.Json;
using Auth.Domain.Aggregates;
using Auth.Domain.Factories.Aggregates;
using Auth.Domain.Repositories;
using Auth.Infrastructure.Persistence.Redis;
using StackExchange.Redis;

namespace Auth.Infrastructure.Persistence.Repositories;

public class RegistrationRepository : CacheAggregateRepoBase<Registration>,IRegistrationRepository
{
    private const string KEY_PREFIX = "registration:";
    private readonly IAggregateCache _aggregateCache;
    private readonly IRegistrationFactory _factory;

    public RegistrationRepository(IAggregateCache aggregateCache,IRegistrationFactory factory)
    {
        _aggregateCache = aggregateCache;
        _factory = factory;
    }
    
    /// <summary>
    /// 把 HashEntry[] 轉成 Aggregate
    /// </summary>
    /// <param name="entries"></param>
    /// <returns></returns>
    protected override Registration HashEntriesToObject(HashEntry[] entries)
    {
        // Processing - 
        var email = entries.FirstOrDefault(x => x.Name == "Email").Value.ToString();
        var code = int.Parse(entries.FirstOrDefault(x => x.Name == "Code").Value!);
        var remainTry = int.Parse(entries.FirstOrDefault(x => x.Name == "RemainTry").Value!);
        var beenVerified = Convert.ToBoolean(entries.FirstOrDefault(x => x.Name == "BeenVerified").Value!);
        var securitystamp = entries.FirstOrDefault(x => x.Name == "SecurityStamp").Value.ToString();
        // Processing - 
        return _factory.Reconstruct(email, remainTry, code, beenVerified, securitystamp);
    }

    /// <summary>
    /// 把 Aggregate 轉成 HashEntry[]
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected override HashEntry[] GetHashEntries(Registration entity)
    {
        // Processing - 
        var hashEntries = new LinkedList<HashEntry>();
        // Processing - 
        hashEntries.AddLast(new HashEntry("Email", entity.Id));
        hashEntries.AddLast(new HashEntry("RemainTry", entity.RemainTry.ToString()));
        hashEntries.AddLast(new HashEntry("Code", entity.Code.ToString()));
        hashEntries.AddLast(new HashEntry("BeenVerified", entity.BeenVerified.ToString())); 
        hashEntries.AddLast(new HashEntry("SecurityStamp", entity.SecurityStamp));
        // Processing - 
        return hashEntries.ToArray();
    }

    /// <summary>
    /// 取得 - 註冊資料 (Email 是註冊資量的 Identity)
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<Registration> GetAsync(string email)
    {
        var entries = await _aggregateCache.GetHashCacheAsync<Registration>(KEY_PREFIX + email);
        if (entries.Any())
        {
            return HashEntriesToObject(entries);
        }
   
        return null;
        
    }

    /// <summary>
    /// 建立 - 註冊資料
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    public async Task AddAsync(Registration registration)
    {
        await _aggregateCache.SaveAsync(KEY_PREFIX + registration.Id, registration, GetHashEntries);
    }

    /// <summary>
    /// 更新 - 註冊資料
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    public async Task UpdateAsync(Registration registration)
    {
        await _aggregateCache.SaveAsync(KEY_PREFIX + registration.Id, registration, GetHashEntries);
    }

    /// <summary>
    /// 刪除 - 註冊資料
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    public async Task DeleteAsync(Registration registration)
    {
        await _aggregateCache.DeleteAsync(KEY_PREFIX + registration.Id, registration);
    }
}