namespace Auth.Presentation.Contract.Feature.Auth;

public class RefreshTokenRequest
{
    /// <summary>
    /// Refresh Token
    /// </summary>
    /// <example>XXX</example>
    public string RefreshToken { get; set; }

    /// <summary>
    /// 裝置型號
    /// </summary>
    /// <example>IPHONE 8</example>
    public string DeviceType { get; set; }
    
    /// <summary>
    ///  裝置令牌
    /// </summary>
    /// <example>null</example>
    public string? DeviceToken { get; set; }
}