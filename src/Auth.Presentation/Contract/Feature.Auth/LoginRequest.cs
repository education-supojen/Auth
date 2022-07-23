namespace Auth.Presentation.Contract.Feature.Auth;

public record LoginRequest
{
    /// <summary>
    /// 使用者 - 郵箱
    /// </summary>
    /// <example>brian71742@gmail.com</example>
    public string Email { get; set; }
    
    /// <summary>
    /// 使用者 - 密碼
    /// </summary>
    /// <example>6XXXBXXX</example>
    public string Password { get; set; }

    /// <summary>
    /// Device Type
    /// </summary>
    /// <example>IPHONE 8</example>
    public string DeviceType { get; set; }

    /// <summary>
    /// Device Token
    /// </summary>
    /// <example>null</example>
    public string? DeviceToken { get; set; }
}