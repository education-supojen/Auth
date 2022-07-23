using Auth.Domain.Aggregates;
using Auth.Domain.Enums;
using Auth.Domain.ValueObjects;

namespace Auth.Domain.Factories.Interface;

public interface IUserFactory
{
    /// <summary>
    /// 註冊新用戶
    /// </summary>
    /// <param name="name"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    User Create(string name, string email, Password password);
    
    /// <summary>
    /// 建立公司成員
    /// </summary>
    /// <param name="name"></param>
    /// <param name="email"></param>
    /// <param name="title"></param>
    /// <param name="boardingTime"></param>
    /// <returns></returns>
    Task<User> CreateAsync(string name, string email, string title, DateOnly boardingTime);
}