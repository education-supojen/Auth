using Auth.Domain.Aggregates;

namespace Auth.Domain.Repositories;

public interface IStaffRepository
{
    /// <summary>
    /// 處存到快取
    /// </summary>
    /// <param name="staff"></param>
    /// <returns></returns>
    Task CacheAddAsync(Staff staff);
    
    /// <summary>
    /// 從快取取得使用者
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Staff?> CacheGetAsync(long id);

    /// <summary>
    /// 刪除快取
    /// </summary>
    /// <param name="staff"></param>
    /// <returns></returns>
    Task CacheDeleteAsync(Staff staff);
    
    /// <summary>
    /// 取得使用者
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Staff?> GetAsync(long id);
    
    /// <summary>
    /// 透過郵箱取得使用者
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<Staff?> GetStaffByEmailAsync(string email);

    /// <summary>
    /// 建立使用者
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    void Add(Staff user);

    /// <summary>
    /// 更新使用者
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    void Update(Staff user);
}