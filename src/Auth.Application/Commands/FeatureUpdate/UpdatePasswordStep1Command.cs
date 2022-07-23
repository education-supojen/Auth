namespace Auth.Application.Commands.FeatureUpdate;

public record UpdatePasswordStep1Command
{
    /// <summary>
    /// 郵箱
    /// </summary>
    public string Email { get; set; }
}