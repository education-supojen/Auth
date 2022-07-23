using Auth.Application.DTO;
using MediatR;

namespace Auth.Application.Commands.FeatureAuth;

public record LoginCommand : IRequest<TokenDto>
{
    /// <summary>
    /// 使用者 - 郵箱
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// 使用者 - 密碼
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Device Type
    /// </summary>
    public string DeviceType { get; set; }

    /// <summary>
    /// Device Token
    /// </summary>
    public string? DeviceToken { get; set; }
}