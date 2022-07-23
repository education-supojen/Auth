namespace Auth.Application.Commands.FeatureUpdate;

public class UpdatePasswordStep2Command
{
    /// <summary>
    /// 郵箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 驗證碼
    /// </summary>
    public int Code { get; set; }
}