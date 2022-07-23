using Auth.Domain.Aggregates;
using Auth.Domain.Factories.Aggregates;
using Auth.Domain.Repositories;
using Auth.Infrastructure.Persistence.Redis;
using StackExchange.Redis;

namespace Auth.Infrastructure.Persistence.Repositories;

public class PasswordUpdateRepository : CacheAggregateRepoBase<PasswordUpdate>, IPasswordUpdateRepository
{
    private const string KEY_PREFIX = "password_update:";
    private readonly IAggregateCache _cache;
    private readonly IPasswordUpdateFactory _factory;

    public PasswordUpdateRepository(IAggregateCache cache,IPasswordUpdateFactory factory) => (_cache,_factory) = (cache,factory);

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="passwordUpdate"></param>
    /// <returns></returns>
    public async Task AddAsync(PasswordUpdate passwordUpdate)
    {
        await _cache.SaveAsync(KEY_PREFIX + passwordUpdate.Id, passwordUpdate, GetHashEntries);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="passwordUpdate"></param>
    /// <returns></returns>
    public async Task UpdateAsync(PasswordUpdate passwordUpdate)
    {
        await _cache.SaveAsync(KEY_PREFIX + passwordUpdate.Id, passwordUpdate, GetHashEntries);
    }

    /// <summary>
    /// 取得
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<PasswordUpdate> GetAsync(string email)
    {
        var entries = await _cache.GetHashCacheAsync<PasswordUpdate>(KEY_PREFIX + email);
        if (entries.Any())
        {
            return HashEntriesToObject(entries);
        }
   
        return null;
    }

    /// <summary>
    /// 刪除
    /// </summary>
    /// <param name="passwordUpdate"></param>
    /// <returns></returns>
    public async Task DeleteAsync(PasswordUpdate passwordUpdate)
    {
        await _cache.DeleteAsync(KEY_PREFIX + passwordUpdate.Id, passwordUpdate);
    }


    #region HashSet 解構 

    protected override PasswordUpdate HashEntriesToObject(HashEntry[] entries)
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

    protected override HashEntry[] GetHashEntries(PasswordUpdate entity)
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

    #endregion
}