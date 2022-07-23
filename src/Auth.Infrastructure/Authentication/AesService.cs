using System.Security.Cryptography;
using System.Text;
using Auth.Application.DTO;
using Auth.Application.DTO.Services;
using Auth.Application.Interfaces.Authentication;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Authentication;

public class AesService : IAesService
{
    private readonly AesSettings _aesSettings;

    public AesService(IOptions<AesSettings> aesOptions)
    {
        _aesSettings = aesOptions.Value;
    }
    
    /// <summary>
    /// 產生一組 (密鑰, Initial vector) 做 AES-CBC 文本加密用的
    /// </summary>
    /// <returns></returns>
    public AesKeyDto AesEncryptKey()
    {
        var key = EncodedRandomString(32); // 256
        Aes cipher = CreateCbcCipher(key);
        var IVBase64 = Convert.ToBase64String(cipher.IV);
        return new AesKeyDto
        {
            Key = key,
            IV = IVBase64
        };
    }
    
    /// <summary>
    /// AES-CBC 文本加密
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string EncryptCBC(string text)
    {
        Aes cipher = CreateCbcCipher(_aesSettings.Key);
        cipher.IV = Convert.FromBase64String(_aesSettings.IV);

        ICryptoTransform cryptTransform = cipher.CreateEncryptor();
        byte[] plaintext = Encoding.UTF8.GetBytes(text);
        byte[] cipherText = cryptTransform.TransformFinalBlock(plaintext, 0, plaintext.Length);

        return Convert.ToBase64String(cipherText);
    }

    /// <summary>
    /// AES-EBC 文本加密 (不需要 Initial Vector 但相對不安全)
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string EncryptECB(string text)
    {
        var base64Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(_aesSettings.IV));
        Aes cipher = CreateEcbCipher(base64Key);
        
        ICryptoTransform cryptTransform = cipher.CreateEncryptor();
        byte[] textBytes = Encoding.UTF8.GetBytes(text);
        byte[] cipherBytes = cryptTransform.TransformFinalBlock(textBytes, 0, textBytes.Length);

        return Convert.ToBase64String(cipherBytes);
    }

    /// <summary>
    /// AES-CBC 文本解密 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string DecryptCBC(string text)
    {
        Aes cipher = CreateCbcCipher(_aesSettings.Key);
        cipher.IV = Convert.FromBase64String(_aesSettings.IV);

        ICryptoTransform cryptTransform = cipher.CreateDecryptor();
        byte[] encryptedBytes = Convert.FromBase64String(text);
        byte[] plainBytes = cryptTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

        return Encoding.UTF8.GetString(plainBytes);
    }
    
    /// <summary>
    /// AES-EBC 文本解密 (不需要 Initial Vector 但相對不安全)
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string DecryptECB(string text)
    {
        var base64Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(_aesSettings.IV));
        Aes cipher = CreateEcbCipher(base64Key);

        ICryptoTransform cryptTransform = cipher.CreateDecryptor();
        byte[] cipherBytes = Convert.FromBase64String(text);
        byte[] plainBytes = cryptTransform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        
        return Encoding.UTF8.GetString(plainBytes);
    }

    #region Helper Function
    /// <summary>
    /// 產生一組 binary 亂數
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    private static byte[] RandomBytes(int length)
    {
        var bytes = new byte[length];
        RandomNumberGenerator.Fill(bytes);
        return bytes;
    }

    /// <summary>
    /// 產生一組 base64 string 亂數
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    private static string EncodedRandomString(int length)
    {
        return Convert.ToBase64String(RandomBytes(length));
    }

    /// <summary>
    /// 產生加密物件(cipher) : AES-CBC - 這種方式,會有 Initial Vector, 每次加密結果可能不一樣, 但比較安全, 竟量用這個
    /// </summary>
    /// <param name="keyBase64"></param>
    /// <returns></returns>
    private static Aes CreateCbcCipher(string keyBase64)
    {
        // Default values: keysize 256, Padding PKC27
        Aes cipher = Aes.Create();
        // Ensure the integrity of the ciphertext if using CBC
        cipher.Mode = CipherMode.CBC;
        cipher.Padding = PaddingMode.ISO10126;
        cipher.Key = Convert.FromBase64String(keyBase64);

        return cipher;
    }

    /// <summary>
    /// 產生加密物件(cipher) : AES-ECB - 這種方式,不會有 Initial Vector, 每次加密結果一樣, 但比較不安全, 非必要別用
    /// </summary>
    /// <param name="keyBase64"></param>
    /// <returns></returns>
    private static Aes CreateEcbCipher(string keyBase64)
    {
        Aes cipher = Aes.Create();
        cipher.Mode = CipherMode.ECB;
        cipher.Padding = PaddingMode.PKCS7;
        cipher.Key = Convert.FromBase64String(keyBase64);
        return cipher;
    }
    
    
    #endregion
}