namespace Auth.Presentation.Contract.Feature.Upt;

public record UpdatePasswordStep2Request
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