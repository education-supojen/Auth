using Auth.Domain.Aggregates;
using Auth.Domain.ValueObjects;

namespace Auth.Domain.Factories.Interface;

public interface ICompanyFactory
{
    /// <summary>
    /// 建立公司
    /// </summary>
    /// <param name="name">公司的名稱</param>
    /// <param name="location">公司的位置</param>
    /// <param name="user">建立公司的使用者</param>
    /// <returns></returns>
    public Company Create(string name, Location location, User user);
}