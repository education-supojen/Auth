using Auth.Domain.Aggregates;

namespace Auth.Domain.Repositories;

public interface IRegistrationRepository
{
    /// <summary>
    /// 取得 - 註冊資料 (Email 是註冊資量的 Identity)
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<Registration> GetAsync(string email);
    
    /// <summary>
    /// 建立 - 註冊資料
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    Task AddAsync(Registration registration);

    /// <summary>
    /// 更新 - 註冊資料
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    Task UpdateAsync(Registration registration);

    /// <summary>
    /// 刪除 - 註冊資料
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    Task DeleteAsync(Registration registration);
}