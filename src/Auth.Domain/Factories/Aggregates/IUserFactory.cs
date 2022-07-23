using Auth.Domain.Aggregates;
using Auth.Domain.Enums;
using Auth.Domain.ValueObjects;

namespace Auth.Domain.Factories.Aggregates;

public interface IUserFactory
{
    /// <summary>
    /// 註冊新用戶
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    User Create(string email, Password password);

    /// <summary>
    /// 給 Redis 快取使用的
    /// </summary>
    /// <param name="id"></param>
    /// <param name="email"></param>
    /// <param name="securityStamp"></param>
    /// <param name="refreshToken"></param>
    /// <param name="refreshTokenExpiryTime"></param>
    /// <returns></returns>
    User ReConstruct(long id, string email, string securityStamp, string refreshToken, DateTime refreshTokenExpiryTime);
}