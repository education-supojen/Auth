using Auth.Application.DTO.Feature.Reg;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Register;

public class StaffRegistrationStep2Command : IRequest<OneOf<RegistrationTokenDto,Failure>>
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