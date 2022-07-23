using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Register;

public class UserRegistrationStep1Command : IRequest<OneOf<bool,Failure>>
{
    /// <summary>
    /// 郵箱
    /// </summary>
    public string Email { get; set; }
}