namespace Auth.Domain.ValueObjects;

public record Password(string HashPassword, string Salt)
{
    /// <summary>
    /// 密碼哈希
    /// </summary>
    public string HashPassword { get; } = HashPassword;

    /// <summary>
    /// 鹽
    /// </summary>
    public string Salt { get; } = Salt;

    /// <summary>
    /// dto -> tuple
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static implicit operator (string salt, string hash)(Password data) => (data.Salt, data.HashPassword);
}