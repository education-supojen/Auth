using Auth.Application.DTO;
using MediatR;

namespace Auth.Application.Commands.FeatureAuth;

public class RefreshTokenCommand : IRequest<TokenDto>
{
    /// <summary>
    /// Access Token
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Refresh Token
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// 裝置型號
    /// </summary>
    public string DeviceType { get; set; }
    
    /// <summary>
    ///  裝置令牌
    /// </summary>
    public string DeviceToken { get; set; }
}