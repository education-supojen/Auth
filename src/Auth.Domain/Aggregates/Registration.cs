using Auth.Domain.Enums;

namespace Auth.Domain.Aggregates;

/// <summary>
/// 
/// </summary>
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
    public int Code { get; private set; }

    /// <summary>
    /// 是否驗證通過
    /// </summary>
    public bool BeenVerified { get; private set; }

    /// <summary>
    /// 給 Infrastructure 端 Persistence 用的
    /// </summary>
    public Registration() { }
    
    /// <summary>
    /// 給 Factory 建立註冊資量用的
    /// </summary>
    /// <param name="email"></param>
    internal Registration(string email)
    {
        Id = email;
        RemainTry = 3;
        Code = new Random().Next(100000, 999999);
        BeenVerified = false;
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
} 