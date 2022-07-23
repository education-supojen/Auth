using Auth.Domain.Aggregates;
using Auth.Domain.Factories.Interface;

namespace Auth.Domain.Factories;


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
}