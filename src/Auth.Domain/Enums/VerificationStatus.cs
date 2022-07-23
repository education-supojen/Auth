namespace Auth.Domain.Enums;

public enum VerificationStatus
{
    /// <summary>
    /// 驗證成功
    /// </summary>
    Success = 0,
    
    /// <summary>
    /// 驗證失敗
    /// </summary>
    Fail = 1,
    
    /// <summary>
    /// 驗證失敗, 並且, 驗證太多次了, 沒有機會了
    /// </summary>
    HaveNoChance = 2
}