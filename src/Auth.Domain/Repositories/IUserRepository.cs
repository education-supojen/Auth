using Auth.Domain.Aggregates;

namespace Auth.Domain.Repositories;

public interface IUserRepository
{
    /// <summary>
    /// 處存到快取
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task CacheAddAsync(User user);
    
    /// <summary>
    /// 從快取取得使用者
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<User?> CacheGetAsync(long id);

    /// <summary>
    /// 刪除快取
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task CacheDeleteAsync(User user);
    
    /// <summary>
    /// 取得使用者
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<User?> GetAsync(long id);
    
    /// <summary>
    /// 透過郵箱取得使用者
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<User?> GetUserByEmailAsync(string email);

    /// <summary>
    /// 建立使用者
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    void Add(User user);

    /// <summary>
    /// 更新使用者
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    void Update(User user);
}