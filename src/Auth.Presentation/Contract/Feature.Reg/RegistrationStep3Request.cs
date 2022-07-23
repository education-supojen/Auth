namespace Auth.Presentation.Contract.Feature.Reg;

public record RegistrationStep3Request
{
    /// <summary>
    /// 註冊用令牌
    /// </summary>
    /// <example>XXX</example>
    public string Token { get; set; }
    
    /// <summary>
    /// 使用者密碼
    /// </summary>
    /// <example>OOO</example>
    public string Password { get; set; }
}