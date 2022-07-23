using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Errors;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IAesService _aesService;

    public JwtTokenGenerator(
        IDateTimeProvider dateTimeProvider, 
        IAesService aesService,
        IOptions<JwtSettings> jwtOptions)
    {
        _dateTimeProvider = dateTimeProvider;
        _aesService = aesService;
        _jwtSettings = jwtOptions.Value;
    }
    
    /// <summary>
    /// 生產 Jwt Token
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="securitystamp"></param>
    /// <returns></returns>
    public string GenerateToken(long userId, string securitystamp)
    {
        // Processing - 
        var nbf = _dateTimeProvider.Now;
        var exp = _dateTimeProvider.Now.AddMinutes(_jwtSettings.ExpiryMinutes);

        // Processing -    
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, _aesService.EncryptCBC(userId.ToString())), // 相當於 ID
            new Claim(JwtRegisteredClaimNames.Jti, _aesService.EncryptCBC(securitystamp)),     // 想當於簽名
            new Claim(JwtRegisteredClaimNames.Exp, exp.ToUniversalTime().ToString()),          // JWT 過期日
            new Claim(JwtRegisteredClaimNames.Nbf, nbf.ToUniversalTime().ToString()),          // JWT 啟用日
        };
        
        // Processing - 產生簽名用的密鑰
        var secretBytes = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
        var key = new SymmetricSecurityKey(secretBytes);
        var signingCredentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

        // Processing - 設定製造 token 的參數
        var token = new JwtSecurityToken(
            issuer:_jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims:claims,
            notBefore:nbf.DateTime,
            expires:exp.DateTime,
            signingCredentials:signingCredentials);

        // Processing - 產生 token 並且回傳
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// 解讀 Access Token Claim 內容
    /// (對 jwt token 做基本驗證)
    /// </summary>
    /// <param name="stream">Jwt Token</param>
    /// <returns></returns>
    public (string sub,string jti) ReadSubAndJti(string stream)
    {
        // Processing - 
        var token = new JwtSecurityTokenHandler().ReadJwtToken(stream);
            
        // Processing - 
        string sub = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
        string jti = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value;
        
        // Processing - 檢查 token 簽名有效性
        var items = stream.Split('.');
        string oldSign = items[2];
        string newSign = HashBy256(secret:_jwtSettings.Secret, $"{items[0]}.{items[1]}");

        // Processing - 
        if (oldSign != newSign) throw Errors.Token.TokenInvalid;

        return (sub, jti);
    }
    
    /// <summary>
    /// HmacSha256 Hash
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    private static string HashBy256(string secret,string text)
    {
        var encoding = new ASCIIEncoding();

        Byte[] textBytes = encoding.GetBytes(text);
        Byte[] keyBytes = encoding.GetBytes(secret);

        Byte[] hashBytes;

        using (HMACSHA256 hash = new HMACSHA256(keyBytes))
            hashBytes = hash.ComputeHash(textBytes);

        return Convert.ToBase64String(hashBytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }
}