using Auth.Domain.Aggregates;

namespace Auth.Domain.Factories.Aggregates;

public interface IRegistrationFactory
{
    /// <summary>
    /// 使用者註冊後建立一比註冊資料
    /// </summary>
    /// <returns></returns>
    Registration Create(string email);

    /// <summary>
    /// 給 Repository 重建資料用的
    /// </summary>
    /// <param name="email"></param>
    /// <param name="remainTry"></param>
    /// <param name="code"></param>
    /// <param name="beenVerified"></param>
    /// <param name="securityStamp"></param>
    /// <returns></returns>
    Registration Reconstruct(string email, int remainTry, int code, bool beenVerified, string securityStamp);
}