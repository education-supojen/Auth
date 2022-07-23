using MediatR;

namespace Auth.Application.Commands.FeatureRegistration;

public class RegistrationStep2Command : IRequest
{
    /// <summary>
    /// 郵箱
    /// </summary>
    /// <example>brian71742@gmail.com</example>
    public string Email { get; set; }
    
    /// <summary>
    /// 驗證碼
    /// </summary>
    /// <example>123456</example>
    public int Code { get; set; }
}