namespace Auth.Domain.ValueObjects;

public record RegistrationInformation
{
    /// <summary>
    /// 使用者 - 郵箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 使用者 - 名稱
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 公司 - 名稱
    /// </summary>
    public string CompanyName { get; set; }

    /// <summary>
    /// 緯度
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// 經度
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// 完整地址
    /// </summary>
    public string FormattedAddress { get; set; }
    
    /// <summary>
    ///使用者密碼
    /// </summary>
    public string Password { get; set; }
}