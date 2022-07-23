using Auth.Domain.Aggregates;
using Auth.Domain.Factories.Aggregates;

namespace Auth.Application.Factories.Aggregates;


public class RegistrationFactory : IRegistrationFactory
{
    /// <summary>
    /// 使用者註冊後建立一比註冊資料
    /// </summary>
    /// <returns></returns>
    public Registration Create(string email)
    {
        return new Registration(email);
    }

    /// <summary>
    /// 給 Repository 重建資料用的
    /// </summary>
    /// <param name="email"></param>
    /// <param name="remainTry"></param>
    /// <param name="code"></param>
    /// <param name="beenVerified"></param>
    /// <param name="securityStamp"></param>
    /// <returns></returns>
    public Registration Reconstruct(string email, int remainTry, int code, bool beenVerified, string securityStamp)
    {
        return new Registration(email, remainTry, code, beenVerified, securityStamp);
    }
    
}