using Auth.Application.DTO;
using MediatR;

namespace Auth.Application.Commands.FeatureRegistration;

public class RegistrationStep3Command : IRequest<TokenDto>
{
    /// <summary>
    /// 使用者 - 郵箱
    /// </summary>
    /// <example>brina71742@gmail.com</example>
    public string Email { get; set; }

    /// <summary>
    /// 使用者 - 名稱
    /// </summary>
    /// <example>蘇柏仁</example>
    public string UserName { get; set; }

    /// <summary>
    /// 公司 - 名稱
    /// </summary>
    /// <example>六國景觀設計有限公司</example>
    public string CompanyName { get; set; }

    /// <summary>
    /// 緯度
    /// </summary>
    /// <example>25.061279028690652</example>
    public double Latitude { get; set; }

    /// <summary>
    /// 經度
    /// </summary>
    /// <example>121.53984066717152</example>
    public double Longitude { get; set; }

    /// <summary>
    /// 完整地址
    /// </summary>
    /// <example>10491台北市中山區龍江路286巷16號1樓</example>
    public string FormattedAddress { get; set; }
    
    /// <summary>
    ///使用者密碼
    /// </summary>
    /// <example>6705Brian</example>
    public string Password { get; set; }

    /// <summary>
    /// 哪種裝置註冊完成
    /// </summary>
    /// <example>IPHONE 8</example>
    public string DeviceType { get; set; }

    /// <summary>
    /// Device Token (Mobile 才有)
    /// </summary>
    /// <example>XXOOXX</example>
    public string? DeviceToken { get; set; }
}