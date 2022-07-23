using System.Security.Cryptography;
using Auth.Application.Interfaces.Authentication;
using Auth.Domain.ValueObjects;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Auth.Infrastructure.Authentication;

public class PasswordHashService : IPasswordHashService
{
        /// <summary>
    /// PBKDF 迭代次數(預設值爲 99999 次)
    /// </summary>
    private static int Iteration = 99999;

    /// <summary>
    /// 哈希函數的選定 (預設是HMACSHA256)
    /// </summary>
    private static KeyDerivationPrf Prf = KeyDerivationPrf.HMACSHA256;

    /// <summary>
    /// 使用者密碼 Pbkdf2 哈希
    /// </summary>
    /// <param name="password"></param>
    /// <returns>
    ///     (byte[] salt, string saltStr, byte[] hashed, string hashedStr)
    ///     (鹽byte形式,鹽base64形式,密碼byte形式,密碼base64形式)
    /// </returns>
    public Password Pbkdf2(string password)
    {
        // Processing - 亂數產生鹽
        byte[] salt = new byte[128 / 8];
        using (var rngCsp = new RNGCryptoServiceProvider())
        {
            rngCsp.GetNonZeroBytes(salt);
        }

        // Processing - 進行哈希
        byte[] hashed = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: Prf,
            iterationCount: Iteration,
            numBytesRequested: 256 / 8);

        // Processing - 把計算的結果回傳
        return new Password(HashPassword: Convert.ToBase64String(hashed), Salt: Convert.ToBase64String(salt));
    }

    /// <summary>
    /// 檢查密碼是否合法(使用者輸入的密碼是否正確)
    /// </summary>
    /// <param name="password"></param>
    /// <param name="hashPassword"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    public bool PasswordValidation(string password, string hashPassword, string salt)
    {
        // Processing - 把鹽從 Base64 轉回 Byte Array
        var saltBytes = Convert.FromBase64String(salt);

        // Processing - 進行哈希
        byte[] hashed = KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: Prf,
            iterationCount: Iteration,
            numBytesRequested: 256 / 8);

        // Processing - 把 pbkdf2 過的密碼轉成 Base64
        var enteredPassword = Convert.ToBase64String(hashed);

        // Processing - 確認密碼次否正確
        return enteredPassword == hashPassword;
    }
}