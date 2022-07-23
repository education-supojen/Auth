using Auth.Domain.Aggregates;

namespace Auth.Domain.Repositories;

public interface IPasswordUpdateRepository
{
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="passwordUpdate"></param>
    /// <returns></returns>
    Task AddAsync(PasswordUpdate passwordUpdate);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="passwordUpdate"></param>
    /// <returns></returns>
    Task UpdateAsync(PasswordUpdate passwordUpdate);

    /// <summary>
    /// 取得
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<PasswordUpdate> GetAsync(string email);

    /// <summary>
    /// 刪除
    /// </summary>
    /// <param name="passwordUpdate"></param>
    /// <returns></returns>
    Task DeleteAsync(PasswordUpdate passwordUpdate);
}