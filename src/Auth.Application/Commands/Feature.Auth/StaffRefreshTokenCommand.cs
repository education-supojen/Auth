using Auth.Application.DTO.Feature.Auth;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Auth;

public record StaffRefreshTokenCommand : IRequest<OneOf<TokenDto,Failure>>
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