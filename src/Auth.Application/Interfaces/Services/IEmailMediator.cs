namespace Auth.Application.Interfaces.Services;

public interface IEmailMediator
{

    /// <summary>
    /// 發送 - 驗證碼 - 後台人員註冊
    /// </summary>
    /// <param name="email"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    Task StaffRegistrationAsync(string email, int code);
    
    /// <summary>
    /// 發送 - 驗證碼 - 使用者註冊
    /// </summary>
    /// <param name="email"></param>
    /// <param name="code"></param>
    Task RegistrationWithEmailAsync(string email, int code);


    /// <summary>
    /// 發送 - 驗證 - 更新密碼
    /// </summary>
    /// <param name="email"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    Task UpdatePasswordAsync(string email, int code);

    /// <summary>
    /// 發送 - 驗證碼 - 登入
    /// </summary>
    /// <param name="email"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    Task LoginWithEmailAsync(string email, int code);

    /// <summary>
    /// 發送 - 新成員初始密碼
    /// </summary>
    /// <param name="email"></param>
    /// <param name="newMemberName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task SendEmployeePasswordAsync(string email, string newMemberName, string password);
}