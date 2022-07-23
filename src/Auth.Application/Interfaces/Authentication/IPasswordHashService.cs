using Auth.Domain.ValueObjects;

namespace Auth.Application.Interfaces.Authentication;

public interface IPasswordHashService
{
    /// <summary>
    /// 使用者密碼 Pbkdf2 哈希
    /// </summary>
    /// <param name="password"></param>
    /// <returns>
    ///     (byte[] salt, string saltStr, byte[] hashed, string hashedStr)
    ///     (鹽byte形式,鹽base64形式,密碼byte形式,密碼base64形式)
    /// </returns>
    Password Pbkdf2(string password);

    /// <summary>
    /// 檢查密碼是否合法(使用者輸入的密碼是否正確)
    /// </summary>
    /// <param name="password"></param>
    /// <param name="hashPassword"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    bool PasswordValidation(string password, string hashPassword, string salt);
}