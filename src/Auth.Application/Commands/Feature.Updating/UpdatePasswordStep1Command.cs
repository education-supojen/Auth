using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Updating;

public record UpdatePasswordStep1Command : IRequest<OneOf<bool,Failure>>
{
    /// <summary>
    /// 郵箱
    /// </summary>
    public string Email { get; set; }
}