namespace Auth.Application.DTO.Feature.Upd;

public record UpdateTokenDto(string Token)
{
    /// <summary>
    /// 令牌
    /// </summary>
    public string Token { get; set; } = Token;
}