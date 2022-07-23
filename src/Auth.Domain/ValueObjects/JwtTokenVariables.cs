namespace Auth.Domain.ValueObjects;

public record JwtTokenVariables(string sub, string jti);