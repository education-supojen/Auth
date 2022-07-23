namespace Auth.Presentation.Contract.Feature.Reg;

public record RegistrationStep1Request
{
    /// <summary>
    /// 郵箱
    /// </summary>
    /// <example>supojen71742@gmail.com</example>
    public string Email { get; set; }
}