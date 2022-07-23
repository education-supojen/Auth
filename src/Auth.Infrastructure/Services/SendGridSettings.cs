namespace Auth.Infrastructure.Services;

public class SendGridSettings
{
    public const string SectionName = "SendGrid";

    /// <summary>
    /// Secret Key
    /// </summary>
    public string Key { get; init; } = null!;
}