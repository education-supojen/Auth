using Auth.Domain.Aggregates;

namespace Auth.Domain.Factories.Interface;

/// <summary>
/// 
/// </summary>
public interface IRegistrationFactory
{
    /// <summary>
    /// 使用者註冊後建立一比註冊資料
    /// </summary>
    /// <returns></returns>
    Registration Create(string email);
}