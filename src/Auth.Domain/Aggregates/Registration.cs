using Auth.Domain.Entities;
using Auth.Domain.Enums;
using Auth.Domain.Errors;
using Auth.Domain.Events.Registrations;
using OneOf;

namespace Auth.Domain.Aggregates;


public class Registration : AggregateRoot, IEntityBase<string>
{
    /// <summary>
    /// 登記郵箱當作 Identity
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// 剩下幾次嘗試機會
    /// </summary>
    public int RemainTry { get; private set; }

    /// <summary>
    /// 驗證碼
    /// </summary>
    public int Code { get; init; }

    /// <summary>
    /// 是否驗證通過
    /// </summary>
    public bool BeenVerified { get; private set; }

    /// <summary>
    /// 驗證成功後, 拿來跟使用者做身份比對用的
    /// </summary>
    public string SecurityStamp { get; private set; }


    /// <summary>
    /// 給 Repository 用的
    /// </summary>
    public Registration(string email, int remainTry, int code, bool beenVerified, string securityStamp)
    {
        Id = email;
        RemainTry = remainTry;
        Code = code;
        BeenVerified = beenVerified;
        SecurityStamp = securityStamp;
    }
    
    /// <summary>
    /// 給 Factory 建立註冊資量用的
    /// </summary>
    /// <param name="email"></param>
    public Registration(string email)
    {
        Id = email;
        RemainTry = 3;
        Code = new Random().Next(100000, 999999);
        BeenVerified = false;
        SecurityStamp = Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// 驗證驗證碼, 成功返回 true, 失敗返回 false 
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public VerificationStatus VerifyCode(int code)
    {
        // Description - 驗證成功
        if (Code == code)
        {
            BeenVerified = true;
            return VerificationStatus.Success;
        }
        
        // Description - 驗證失敗
        if (--RemainTry > 0)
        {
            return VerificationStatus.Fail;
        }

        // Description - 驗證失敗, 並且, 驗證太多次, 不給機會了
        return VerificationStatus.HaveNoChance;
    }

    /// <summary>
    /// 是否驗證完畢,已經準備好可以註冊了
    /// </summary>
    /// <param name="securityStamp"></param>
    public OneOf<bool,Failure> IfReadyToRegister(string securityStamp)
    {
        // Processing - 檢查令牌是否正確
        if (SecurityStamp != securityStamp) return Failures.Registration.TokenInvalid;
        
        // Processing - 驗證碼不正確
        if (BeenVerified == false) return Failures.Registration.Null;

        // Mission Complete
        return true;
    }

    /// <summary>
    /// 啟動 - 註冊會員 - 事件
    /// </summary>
    public void RegisterForUser(string password)
    {
        // Processing - 建立註冊事件
        var @event = new ReadyToRegisterUserEvent(Id, password);
        // Processing - 註冊事件存入 Aggregate 當中
        AddEvent(@event);
    }

    /// <summary>
    /// 啟動 - 註冊後台人員 - 事件
    /// </summary>
    /// <param name="password"></param>
    public void RegisterForStaff(string password)
    {
        // Processing - 建立註冊事件
        var @event = new ReadyToRegisterStaffEvent(Id, password);
        // Processing - 註冊事件存入 Aggregate 當中
        AddEvent(@event);
    }
} 