using Auth.Application.Interfaces.Authentication;
using Auth.Domain.Aggregates;
using Auth.Domain.Errors;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Authentication;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly IAesService _aesService;
    private readonly JwtSettings _jwtSettings;

    public RefreshTokenGenerator(IAesService aesService,IOptions<JwtSettings> jwtOptions)
    {
        _aesService = aesService;
        _jwtSettings = jwtOptions.Value;
    }

    /// <summary>
    /// 計算 refresh Token
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public string GenerateRefreshToken(User user)
    {
        return _aesService.EncryptECB(user.RefreshToken);
    }
}