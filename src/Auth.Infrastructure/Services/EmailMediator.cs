using Auth.Application.Interfaces.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Auth.Infrastructure.Services;

public class EmailMediator : IEmailMediator
{
    private readonly SendGridSettings _settings;

    public EmailMediator(IOptions<SendGridSettings> settings)
    {
        _settings = settings.Value;
    }

    /// <summary>
    /// 發送 - 驗證碼 - 後台人員註冊
    /// </summary>
    /// <param name="email"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task StaffRegistrationAsync(string email, int code)
    {
        var client = new SendGridClient(_settings.Key);
        const string subject = $"後台人員註冊";
        var from = new EmailAddress("brian.su@plantsist.com");
        var to = new EmailAddress("brian71742@gmail.com");
        var content = $"驗證碼是 {code}, 註冊郵箱是 {email}";
        var htmlContent = $"<strong>{content}</strong>";

        var message = MailHelper.CreateSingleEmail(from, to, subject,content,htmlContent);
        await client.SendEmailAsync(message);
    }

    /// <summary>
    /// 發送 - 郵箱 - 驗證碼
    /// </summary>
    /// <param name="email"></param>
    /// <param name="code"></param>
    public async Task RegistrationWithEmailAsync(string email, int code)
    {
        var client = new SendGridClient(_settings.Key);
        const string subject = $"柏哥ㄟ店 - 驗證碼";
        var from = new EmailAddress("brian.su@plantsist.com");
        var to = new EmailAddress(email);
        var content = $"驗證碼是 {code}";
        var htmlContent = $"<strong>{content}</strong>";

        var message = MailHelper.CreateSingleEmail(from, to, subject,content,htmlContent);
        await client.SendEmailAsync(message);
    }

    /// <summary>
    /// 發送 - 驗證 - 更新密碼
    /// </summary>
    /// <param name="email"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task UpdatePasswordAsync(string email, int code)
    {
        var client = new SendGridClient(_settings.Key);
        const string subject = $"密碼更新 - 驗證碼";
        var from = new EmailAddress("brian.su@plantsist.com");
        var to = new EmailAddress(email);
        var content = $"驗證碼是 {code}";
        var htmlContent = $"<strong>{content}</strong>";

        var message = MailHelper.CreateSingleEmail(from, to, subject,content,htmlContent);
        await client.SendEmailAsync(message);
    }

    
    public async Task LoginWithEmailAsync(string email, int code)
    {
        var client = new SendGridClient(_settings.Key);
        const string subject = $"登入 - 驗證碼";
        var from = new EmailAddress("brian.su@plantsist.com");
        var to = new EmailAddress(email);
        var content = $"驗證碼是 {code}";
        var htmlContent = $"<strong>{content}</strong>";

        var message = MailHelper.CreateSingleEmail(from, to, subject,content,htmlContent);
        await client.SendEmailAsync(message);
    }

    /// <summary>
    /// 發送 - 新成員初始密碼
    /// </summary>
    /// <param name="email"></param>
    /// <param name="newMemberName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task SendEmployeePasswordAsync(string email, string newMemberName, string password)
    {
        var client = new SendGridClient(_settings.Key);
        var subject = $"{newMemberName}的初始密碼";
        var from = new EmailAddress("brian.su@plantsist.com");
        var to = new EmailAddress(email);
        var content = $"密碼是 {password}";
        var htmlContent = $"<strong>{content}</strong>";

        var message = MailHelper.CreateSingleEmail(from, to, subject,content,htmlContent);
        await client.SendEmailAsync(message);  
    }
}