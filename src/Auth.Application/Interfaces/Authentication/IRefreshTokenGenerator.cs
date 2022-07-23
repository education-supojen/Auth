using Auth.Domain.Aggregates;

namespace Auth.Application.Interfaces.Authentication;

public interface IRefreshTokenGenerator
{
    /// <summary>
    /// 計算 refresh Token
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    string GenerateRefreshToken(User user);
    
    /// <summary>
    /// 計算 refresh Token
    /// </summary>
    /// <param name="staff"></param>
    /// <returns></returns>
    string GenerateRefreshToken(Staff staff);
}