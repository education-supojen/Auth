using System.Globalization;
using Auth.Domain.Aggregates;
using Auth.Domain.Errors;
using Auth.Domain.Factories.Aggregates;
using Auth.Domain.Repositories;
using Auth.Domain.ValueObjects;
using Auth.Infrastructure.Persistence.EF;
using Auth.Infrastructure.Persistence.Redis;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Auth.Infrastructure.Persistence.Repositories;

public class StaffRepository :  CacheAggregateRepoBase<Staff>,IStaffRepository
{
    private const string KEY_PREFIX = "staff_auth:";
    private readonly DbSet<Staff> _users;
    private readonly IStaffFactory _factory;
    private readonly IAggregateCache _cache;
    
    public StaffRepository(AppDbContext appDbContext,IStaffFactory factory,IAggregateCache cache)
    {
        _users = appDbContext.Staff;
        _factory = factory;
        _cache = cache;
    }
    
    /// <summary>
    /// 註冊最後一步, 新增使用者
    /// </summary>
    /// <param name="staff"></param>
    public void Add(Staff staff)
    {
        _users.Add(staff);
    }

    /// <summary>
    /// 更新使用者
    /// </summary>
    /// <param name="staff"></param>
    public void Update(Staff staff)
    {
        _users.Update(staff);
    }
    
    /// <summary>
    /// 取得使用者
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Staff?> GetAsync(long id)
    {
        return await _users.SingleOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// 使用 Email 找到以註冊的使用者
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<Staff?> GetStaffByEmailAsync(string email)
    {
        return await _users.SingleOrDefaultAsync(x => x.Email == email);
    }

    /// <summary>
    /// 處存到快取
    /// </summary>
    /// <param name="staff"></param>
    public async Task CacheAddAsync(Staff staff)
    {
        await _cache.SaveAsync(KEY_PREFIX + staff.Id, staff, GetHashEntries);
    }

    /// <summary>
    /// 取得
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Staff?> CacheGetAsync(long id)
    {
        // Processing - 從 Redis 中讀取使用者資訊
        var entries = await _cache.GetHashCacheAsync<Staff>(KEY_PREFIX + id);
        
        // Processing - 如果 Cache 內沒有資料
        if (!entries.Any())
        {
            // Processing - 從資料庫讀取使用者
            var user = await _users.FindAsync(id);
            
            // Processing - 讀不到資料拋出錯誤(代表無法認證身份)
            if (user == null) return null;
            
            // Processing - 把資料處存到 Redis Cache 當中
            await CacheAddAsync(user);
        }
        
        // Processing - 把 Redis Hash 讀到的資料從建成使用者模型返回
        return HashEntriesToObject(entries);
    }

    /// <summary>
    /// 刪除快取
    /// </summary>
    /// <param name="staff"></param>
    /// <returns></returns>
    public async Task CacheDeleteAsync(Staff staff)
    {
        await _cache.DeleteAsync(KEY_PREFIX + staff.Id, staff);
    }
    
    /// <summary>
    /// Hash Set Fields 轉成此用者模型
    /// </summary>
    /// <param name="entries"></param>
    /// <returns></returns>
    /// <exception cref="Failure"></exception>
    protected override Staff HashEntriesToObject(HashEntry[] entries)
    {
        // Processing - 解讀使用者 ID
        var parseId = long.TryParse(entries.FirstOrDefault(x => x.Name == "Id").Value.ToString(), out var id);
        if (parseId is false) throw new FormatException();

        // Processing - 解讀郵箱
        var email = entries.FirstOrDefault(x => x.Name == "Email").Value.ToString();

        // Processing - 解讀 SecurityStamp
        var securitystamp = entries.FirstOrDefault(x => x.Name == "SecurityStamp").Value.ToString();
        
        // Processing - 解讀 Refresh Token
        var refreshToken = entries.FirstOrDefault(x => x.Name == "RefreshToken").Value.ToString();
        
        // Processing - 解讀 Refresh Token Expiry Time
        var parseExpiryTime = DateTime.TryParse(entries.FirstOrDefault(x => x.Name == "RefreshTokenExpiryTime").Value.ToString(), out var expiryTime);
        if (parseExpiryTime is false) throw new FormatException();

        // Processing - 重建使用者
        var user = _factory.ReConstruct(id, email, securitystamp, refreshToken, expiryTime);
        
        // Processing - 回傳使用者
        return user;
    }
    
    /// <summary>
    /// 使用者模型轉成 HashSet Fields
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected override HashEntry[] GetHashEntries(Staff entity)
    {
        // Processing - HashSet Fields
        var hashEntries = new LinkedList<HashEntry>();
        
        // Processing - 把需要用到的資料煮成 HashSet Fields 
        hashEntries.AddLast(new HashEntry("Id", entity.Id));
        hashEntries.AddLast(new HashEntry("Email", entity.Email));
        hashEntries.AddLast(new HashEntry("SecurityStamp", entity.SecurityStamp));
        hashEntries.AddLast(new HashEntry("RefreshToken", entity.RefreshToken));
        hashEntries.AddLast(new HashEntry("RefreshTokenExpiryTime", entity.RefreshTokenExpiryTime.ToString(CultureInfo.InvariantCulture)));

        // Processing - 返回 HashSet Fields
        return hashEntries.ToArray();
    }
    
}