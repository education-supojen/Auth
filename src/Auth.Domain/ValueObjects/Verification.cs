namespace Auth.Domain.ValueObjects;

public record Verification
{
    /// <summary>
    /// 使用者郵箱
    /// </summary>
    public string Email { get; }
    
    /// <summary>
    /// 驗證碼
    /// </summary>
    public int Code { get; } = new Random().Next(100000, 999999);

    /// <summary>
    /// 驗證通過
    /// </summary>
    public bool Success { get; } = false;

    /// <summary>
    /// 剩餘次數
    /// </summary>
    public int RemainTry { get; } = 3;

    /// <summary>
    /// JsonSerializer 用
    /// </summary>
    public Verification() { }

    /// <summary>
    /// 註冊
    /// </summary>
    /// <param name="email"></param>
    public Verification(string email) => Email = email;
    
    /// <summary>
    /// 驗證失敗
    /// </summary>
    /// <param name="verification"></param>
    /// <exception cref="Exception"></exception>
    public Verification(Verification verification)
    {
        if(--RemainTry < 0) throw new Exception("重新嘗試註冊");
        Email = verification.Email;
        Code = verification.Code;
        RemainTry = verification.RemainTry;
    }
}