using MediatR;

namespace Auth.Application.Commands.FeatureRegistration;

public class RegistrationStep1Command : IRequest
{
    /// <summary>
    /// 郵箱
    /// </summary>
    /// <example>supojen71742@gmail.com</example>
    public string Email { get; set; }
}