using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Register;

public class UserRegistrationStep3Command : IRequest<OneOf<bool,Failure>>
{
    /// <summary>
    /// 註冊用令牌
    /// </summary>
    public string Token { get; set; }
    
    /// <summary>
    ///使用者密碼
    /// </summary>
    public string Password { get; set; }
}