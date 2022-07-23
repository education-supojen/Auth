using Auth.Application.Interfaces.Authentication;
using Auth.Domain.ValueObjects;


namespace Auth.Infrastructure.Services;

public class PasswordGenerator : IPasswordGenerator
{
    /// <summary>
    /// 亂數產生密碼
    /// </summary>
    /// <returns></returns>
    public string Generate()
    {
        return CreateRandomPassword(10);
    }

    private static string CreateRandomPassword(int PasswordLength)
    {
        var _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
        var randNum = new Random();
        var chars = new char[PasswordLength];
        var allowedCharCount = _allowedChars.Length;
        for (int i = 0; i < PasswordLength; i++)
        {
            chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
        }
        return new string(chars);
    }
}