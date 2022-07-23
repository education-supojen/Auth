namespace Auth.Infrastructure.Authentication;

public class AesSettings
{
    public const string SectionName = "AesSettings";

    /// <summary>
    /// Secret Key
    /// </summary>
    public string Key { get; init; } = null!;

    /// <summary>
    /// Initial Vector
    /// </summary>
    public string IV { get; init; } = null!;
}