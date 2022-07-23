using Auth.Application.DTO;

namespace Auth.Application.Interfaces.Authentication;

public interface IAesService
{
    /// <summary>
    /// 產生一組 (密鑰, Initial vector) 做 AES-CBC 文本加密用的
    /// </summary>
    /// <returns></returns>
    AesKeyDto AesEncryptKey();
    
    /// <summary>
    /// AES-CBC 文本加密
    /// </summary>
    /// <param name="text">文本</param>
    /// <returns></returns>
    string EncryptCBC(string text);

    /// <summary>
    /// AES-EBC 文本加密 (不需要 Initial Vector 但相對不安全)
    /// </summary>
    /// <param name="text">文本</param>
    /// <returns></returns>
    string EncryptECB(string text);

    /// <summary>
    /// AES-CBC 文本解密 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    string DecryptCBC(string text);

    /// <summary>
    /// AES-EBC 文本解密 (不需要 Initial Vector 但相對不安全)
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    string DecryptECB(string text);
}