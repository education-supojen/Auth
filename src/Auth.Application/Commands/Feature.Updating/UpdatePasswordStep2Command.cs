using Auth.Application.DTO.Feature.Upd;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Updating;

public class UpdatePasswordStep2Command : IRequest<OneOf<UpdateTokenDto,Failure>>
{
    /// <summary>
    /// 郵箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 驗證碼
    /// </summary>
    public int Code { get; set; }
}