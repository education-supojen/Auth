using System.Security.AccessControl;
using Auth.Domain.Enums;
using Auth.Domain.ValueObjects;

namespace Auth.Domain.Aggregates;

public class User : AggregateRoot, IEntityBase<long>
{
    private const int RefreshTokenExpiredMinutes = 3600;
    
    /// <summary>
    /// 使用者 - ID
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// 使用者 - 代號
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    ///  使用者 - 郵箱
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// 郵箱是否確認了
    /// </summary>
    public bool EmailConfirmed { get; private set; }

    /// <summary>
    /// 密碼哈希
    /// </summary>
    public string PasswordHash { get; private set; }

    /// <summary>
    /// 鹽
    /// </summary>
    public string Salt { get; private set; }

    /// <summary>
    /// 入職時間
    /// </summary>
    public DateOnly BoardingTime { get; set; }

    /// <summary>
    /// 職稱
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 使用者姓名
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// 安全鎖
    /// </summary>
    public string SecurityStamp { get; private set; }

    /// <summary>
    /// Refresh Token
    /// </summary>
    public string RefreshToken { get; private set; }
    
    /// <summary>
    /// Refresh Token 過期時間
    /// </summary>
    public DateTime RefreshTokenExpiryTime { get; private set; }
    
    /// <summary>
    /// 權限
    /// </summary>
    public Permission Permission { get; set; }

    /// <summary>
    /// 主管
    /// </summary>
    public virtual User Manager { get; set; }

    /// <summary>
    /// 登入裝置令牌
    /// </summary>
    public string DeviceToken { get; set; }

    /// <summary>
    /// 登入裝置型號
    /// </summary>
    public string DeviceType { get; set; }

    /// <summary>
    /// 屬於哪家公司
    /// </summary>
    public virtual Company Company { get; set; }
    
    /// <summary>
    /// 下屬
    /// </summary>
    public virtual ICollection<User> Subordinates { get; set; }

    /// <summary>
    /// 部門
    /// </summary>
    public virtual Department Department { get; set; }

    /// <summary>
    /// 工作表
    /// </summary>
    public virtual Schedule Schedule { get; set; }

    /// <summary>
    /// 輪班
    /// </summary>
    public virtual Shift Shift { get; set; }

    /// <summary>
    /// 打卡設定
    /// </summary>
    public virtual PunchGroup PunchGroup { get; set; }
    
    /// <summary>
    /// 申請後台帳號
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="title"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    public User(
        long id,
        string name, 
        string title,
        string email, 
        Password password)
    {
        Id = id;
        Name = name;
        Title = title;
        Email = email;
        EmailConfirmed = true;
        BoardingTime = DateOnly.FromDateTime(DateTime.Now);
        PasswordHash = password.HashPassword;
        Salt = password.Salt;
        AssignNumberToUser();
        SecurityStamp = Guid.NewGuid().ToString("N");
        RefreshToken = Guid.NewGuid().ToString("N");
        RefreshSecurityParams();
    }

    /// <summary>
    /// 後台人員建立使用者
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="boardingTime"></param>
    /// <param name="title"></param>
    public User(
        long id, 
        string name, 
        string email,
        Password password,
        DateOnly boardingTime, 
        string title)
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = password.HashPassword;
        Salt = password.Salt;
        BoardingTime = boardingTime;
        Title = title;
        AssignNumberToUser();
        SecurityStamp = Guid.NewGuid().ToString("N");
        RefreshToken = Guid.NewGuid().ToString("N");
        RefreshSecurityParams();
    }

    /// <summary>
    /// 指定編號給使用者
    /// </summary>
    public void AssignNumberToUser()
    {
        // Processing - 初始化編號
        var random = new Random();
        Number = random.Next(1, 10000).ToString();
        
        // Processing - 若為公司第一人, 不需要去考慮編號重複到的問題
        if (Company == null) return;
        
        // Processing - 若編號重複, 重新計算編號
        while (Company.Users.Any(x => x.Number == Number))
        {
            Number = random.Next(1, 10000).ToString();
        }
    }

    /// <summary>
    /// 指定使用者進入部門
    /// </summary>
    /// <param name="department"></param>
    public void AssignUserToDepartment(Department department) => Department = department;

    /// <summary>
    /// 指定使用者進入工作表
    /// </summary>
    /// <param name="schedule"></param>
    public void AssignUserToSchedule(Schedule schedule) => Schedule = schedule;

    /// <summary>
    /// 指定使用者進入輪班
    /// </summary>
    /// <param name="shift"></param>
    public void AssignUserToShift(Shift shift) => Shift = shift;
    
    /// <summary>
    /// 指定使用者進入打卡群組
    /// </summary>
    /// <param name="punchGroup"></param>
    public void AssignUserToPunchGroup(PunchGroup punchGroup) => PunchGroup = punchGroup;
    
    /// <summary>
    /// 指定主管給使用者
    /// </summary>
    /// <param name="manager"></param>
    public void AssignUserAsSubordinateOf(User manager)
    {
        Manager = manager;
    }
    
    /// <summary>
    /// 更新 Token 所需要得參數, User 有任何改變都要來一遍
    /// </summary>
    public void RefreshSecurityParams(int refreshExpiryMinutes = 3600)
    {
        SecurityStamp = Guid.NewGuid().ToString("N");
        RefreshToken = Guid.NewGuid().ToString("N");
        RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshExpiryMinutes);
    }

    /// <summary>
    ///  登入
    /// </summary>
    /// <param name="deviceType"></param>
    /// <param name="deviceToken"></param>
    public void Login(string deviceType, string deviceToken)
    {
        RefreshSecurityParams();
        DeviceType = deviceType;
        DeviceToken = deviceToken;
    }

    /// <summary>
    /// 設置裝置令牌(每次請求的時候,會觸發一次)
    /// </summary>
    /// <param name="deviceType"></param>
    /// <param name="deviceToken"></param>
    public void SetDeviceToken(string deviceType, string deviceToken)
    {
        DeviceToken = deviceToken;
        if (DeviceType != deviceType)
        {
            DeviceType = null; 
            DeviceToken = null;
            throw Errors.Errors.Token.TokenInvalid;
        }
        
    }

    /// <summary>
    /// 登出
    /// </summary>
    public void Logout()
    {
        RefreshSecurityParams();
        DeviceType = null;
        DeviceToken = null;
    }
}