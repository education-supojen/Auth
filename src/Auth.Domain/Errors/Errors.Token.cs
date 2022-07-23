namespace Auth.Domain.Errors;

public static partial class Errors
{
    public static class Token
    {
        public static Error TokenExpire => Error.Unauthorized("Token.TokenExpire", "Token 過期");

        public static Error TokenInvalid => Error.Unauthorized("Token.TokenInvalid", "Token 不合法");
    }
}