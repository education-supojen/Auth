namespace Auth.Application.Commands.FeatureUpdate;

public class UpdatePasswordStep3Command
{
    /// <summary>
    /// 郵箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 密碼
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 密碼確認
    /// </summary>
    public string PasswordConfirmed { get; set; }
}