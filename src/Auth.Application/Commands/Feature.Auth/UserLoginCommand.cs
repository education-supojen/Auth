using Auth.Application.DTO.Feature.Auth;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Auth;

public record UserLoginCommand : IRequest<OneOf<TokenDto,Failure>>
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