using Auth.Domain.ValueObjects;

namespace Auth.Application.Interfaces.Authentication;

public interface IPasswordGenerator
{
    /// <summary>
    /// 亂數產生密碼
    /// </summary>
    /// <returns></returns>
    string Generate();
}