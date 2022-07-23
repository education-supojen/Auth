using Auth.Domain.Errors;
using Auth.Domain.ValueObjects;
using OneOf;

namespace Auth.Application.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    /// <summary>
    /// 生產 Jwt Token
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="securitystamp"></param>
    /// <returns></returns>
    public string GenerateToken(long userId, string securitystamp);

    /// <summary>
    /// 生產 Jwt Token
    /// </summary>
    /// <param name="sub"></param>
    /// <param name="jti"></param>
    /// <returns></returns>
    public string GenerateToken(string sub, string jti);

    /// <summary>
    /// 解讀 Access Token Sub(身份 ID) and Jti(簽名)
    /// (對 jwt token 做基本驗證)
    /// </summary>
    /// <param name="stream">Jwt Token</param>
    /// <returns></returns>
    public OneOf<JwtTokenVariables, Failure> ReadSubAndJti(string stream);
}