namespace Auth.Application.DTO.Feature.Reg;

public record RegistrationTokenDto(string Token)
{
    /// <summary>
    /// 令牌
    /// </summary>
    public string Token { get; set; } = Token;
}