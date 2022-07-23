namespace Auth.Presentation.Contract.Feature.Upt;

public record UpdatePasswordStep1Request
{
    /// <summary>
    /// 郵箱
    /// </summary>
    /// <example>brian71742@gmail.com</example>
    public string Email { get; set; }
}