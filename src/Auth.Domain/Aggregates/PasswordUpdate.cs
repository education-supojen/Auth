using Auth.Domain.Entities;
using Auth.Domain.Enums;
using Auth.Domain.Errors;
using Auth.Domain.Events.Updating;
using OneOf;

namespace Auth.Domain.Aggregates;

public class PasswordUpdate : AggregateRoot, IEntityBase<string>
{
    /// <summary>
    /// 使用者的 Email 
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// 剩下幾次嘗試機會
    /// </summary>
    public int RemainTry { get; private set; }

    /// <summary>
    /// 驗證碼
    /// </summary>
    public int Code { get; private set; }

    /// <summary>
    /// 是否驗證通過
    /// </summary>
    public bool BeenVerified { get; private set; }

    /// <summary>
    /// 驗證成功後, 拿來跟使用者做身份比對用的
    /// </summary>
    public string SecurityStamp { get; set; }
    
    /// <summary>
    /// 給 Repository 用的
    /// </summary>
    public PasswordUpdate(string email, int remainTry, int code, bool beenVerified, string securityStamp)
    {
        Id = email;
        RemainTry = remainTry;
        Code = code;
        BeenVerified = beenVerified;
        SecurityStamp = securityStamp;
    }
    
    /// <summary>
    /// 申請密碼更新的
    /// </summary>
    /// <param name="email"></param>
    public PasswordUpdate(string email)
    {
        Id = email;
        RemainTry = 3;
        Code = new Random().Next(100000, 999999);
        BeenVerified = false;
        SecurityStamp = Guid.NewGuid().ToString("N");
        // Processing - 建立申請更換郵箱事件
        var @event = new ApplyUpdatePasswordEvent(email, Code);
        AddEvent(@event);
    }

    /// <summary>
    /// 驗證驗證碼, 成功返回 true, 失敗返回 false 
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public void VerifyCode(int code)
    {
        // Variable - 驗證狀態
        VerificationStatus status;
        
        // Description - 驗證成功
        if (Code == code)
        {
            BeenVerified = true;
            status =  VerificationStatus.Success;
        }
        // Description - 驗證失敗
        else if (--RemainTry > 0)
        {
            status =  VerificationStatus.Fail;
        }
        // Description - 驗證失敗, 並且, 驗證太多次, 不給機會了
        else
        {
            status = VerificationStatus.HaveNoChance;
        }
        
        // Description - 建立事件
        var @event = new VerifyCodeForUpdatePasswordEvent(this,status);
        AddEvent(@event);
    }
    
    /// <summary>
    /// 是否驗證完畢,已經準備好可以註冊了
    /// </summary>
    /// <param name="securityStamp"></param>
    public OneOf<bool,Failure> IfReadyToUpdatePassword(string securityStamp)
    {
        // Processing - 檢查令牌是否正確
        if (SecurityStamp != securityStamp) return Failures.Update.TokenInvalid;
        
        // Processing - 驗證碼不正確
        if (BeenVerified == false) return Failures.Update.Null;
        
        // Mission Complete
        return true;
    }

    /// <summary>
    /// 啟動 - 註冊 - 事件
    /// </summary>
    public void Update(User user, string password)
    {
        // Processing - 建立註件
        var @event = new ReadyToUpdatePasswordEvent(user, password);
        // Processing - 事件存入 Aggregate 當中
        AddEvent(@event);
    }
}