using Auth.Application.Interfaces.Authentication;
using Auth.Domain.Aggregates;
using Auth.Domain.Errors;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Authentication;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly IAesService _aesService;

    public RefreshTokenGenerator(IAesService aesService) => _aesService = aesService;
    
    /// <summary>
    /// 計算 refresh Token
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public string GenerateRefreshToken(User user)
    {
        return _aesService.EncryptECB(user.RefreshToken);
    }

    /// <summary>
    /// 計算 refresh Token
    /// </summary>
    /// <param name="staff"></param>
    /// <returns></returns>
    public string GenerateRefreshToken(Staff staff)
    {
        return _aesService.EncryptECB(staff.RefreshToken);
    }
}