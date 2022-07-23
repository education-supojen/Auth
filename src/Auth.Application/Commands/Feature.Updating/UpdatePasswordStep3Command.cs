using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Updating;

public class UpdatePasswordStep3Command : IRequest<OneOf<bool,Failure>>
{
    /// <summary>
    /// 更新密碼令牌
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// 密碼
    /// </summary>
    public string Password { get; set; }
}