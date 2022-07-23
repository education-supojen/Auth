namespace Auth.Presentation.Contract.Feature.Upt;

public record UpdatePasswordStep3Request
{
    /// <summary>
    /// 更新密碼令牌
    /// </summary>
    /// <example>XXX</example>
    public string Token { get; set; }

    /// <summary>
    /// 密碼
    /// </summary>
    /// <example>OOO</example>
    public string Password { get; set; }
}