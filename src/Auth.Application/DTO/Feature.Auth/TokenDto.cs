namespace Auth.Application.DTO.Feature.Auth;

public record TokenDto
{
    /// <summary>
    /// Access Token
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Refresh Token
    /// </summary>
    public string RefreshToken { get; set; }
}