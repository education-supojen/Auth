using Auth.Domain.Entities;
using Auth.Domain.Errors;
using Auth.Domain.Events.Staffs;
using Auth.Domain.ValueObjects;
using OneOf;

namespace Auth.Domain.Aggregates;

public class Staff : AggregateRoot , IEntityBase<long>
{
    private const int RefreshTokenExpiredMinutes = 3600;
    
    /// <summary>
    /// 使用者 - ID
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  使用者 - 郵箱
    /// </summary>
    public string Email { get; internal set; }

    /// <summary>
    /// 郵箱是否確認了
    /// </summary>
    public bool EmailConfirmed { get; internal set; }

    /// <summary>
    /// 國家碼
    /// </summary>
    public string CallingCode { get; set; }

    /// <summary>
    /// 電話號碼
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 電話是否否確認過了
    /// </summary>
    public bool PhoneConfirmed { get; set; }
    
    /// <summary>
    /// 密碼
    /// </summary>
    public Password Password { get; internal set; }

    /// <summary>
    /// 安全鎖
    /// </summary>
    public string SecurityStamp { get; internal set; }

    /// <summary>
    /// Refresh Token
    /// </summary>
    public string RefreshToken { get; internal set; }
    
    /// <summary>
    /// Refresh Token 過期時間
    /// </summary>
    public DateTime RefreshTokenExpiryTime { get; internal set; }

    /// <summary>
    /// 登入裝置令牌
    /// </summary>
    public string? DeviceToken { get; internal set; }

    /// <summary>
    /// 登入裝置型號
    /// </summary>
    public string DeviceType { get; internal set; }
    
    /// <summary>
    /// 給 ORM 使用的
    /// </summary>
    public Staff() { }

    /// <summary>
    /// 給 Repository 當中 Redis Cache
    /// </summary>
    /// <param name="id"></param>
    /// <param name="email"></param>
    /// <param name="securityStamp"></param>
    /// <param name="refreshToken"></param>
    /// <param name="refreshTokenExpiryTime"></param>
    public Staff(long id, string email, string securityStamp, string refreshToken, DateTime refreshTokenExpiryTime)
    {
        Id = id;
        Email = email;
        SecurityStamp = securityStamp;
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = refreshTokenExpiryTime;
    }

    /// <summary>
    /// 建立使用者
    /// </summary>
    /// <param name="id"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    public Staff(long id, string email, Password password)
    {
        Id = id;
        Email = email;
        EmailConfirmed = true;
        Password = password;
        SecurityStamp = Guid.NewGuid().ToString("N");
        RefreshToken = Guid.NewGuid().ToString("N");
        RefreshSecurityParams();
    }
    
    /// <summary>
    /// 更新 Token 所需要得參數, User 有任何改變都要來一遍
    /// </summary>
    public void RefreshSecurityParams(int refreshExpiryMinutes = 3600)
    {
        SecurityStamp = Guid.NewGuid().ToString("N");
        RefreshToken = Guid.NewGuid().ToString("N");
        RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshExpiryMinutes);
        var @event = new StaffLoginEvent(this);
        AddEvent(@event);
    }

    /// <summary>
    ///  登入
    /// </summary>
    /// <param name="deviceType"></param>
    /// <param name="deviceToken"></param>
    public void Login(string deviceType, string? deviceToken)
    {
        RefreshSecurityParams();
        DeviceType = deviceType;
        DeviceToken = deviceToken;
        var @event = new StaffLoginEvent(this);
        AddEvent(@event);
    }

    /// <summary>
    /// 設置裝置令牌(每次請求的時候,會觸發一次)
    /// </summary>
    /// <param name="deviceType"></param>
    /// <param name="deviceToken"></param>
    public OneOf<bool,Failure> SetDeviceToken(string deviceType, string? deviceToken)
    {
        // Processing- 
        DeviceToken = deviceToken;
        
        // Processing - 
        if (DeviceType != deviceType)
        {
            DeviceType = null; 
            DeviceToken = null;
            return Failures.Token.TokenInvalid;
        }
        
        // Mission Complete
        return true;
    }

    /// <summary>
    /// 更改密碼
    /// </summary>
    /// <param name="password"></param>
    public void EditPassword(Password password) => Password = password;

    /// <summary>
    /// 登出
    /// </summary>
    public void Logout()
    {
        RefreshSecurityParams();
        DeviceType = null;
        DeviceToken = null;
        var @event = new StaffLogoutEvent(this);
        AddEvent(@event);
    }
}