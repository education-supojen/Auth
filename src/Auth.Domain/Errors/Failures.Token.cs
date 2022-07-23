namespace Auth.Domain.Errors;

public static partial class Failures
{
    public static class Token
    {
        public static Failure TokenExpire => Failure.UnAuthorized("Token.TokenExpire", "Token 過期");

        public static Failure TokenInvalid => Failure.UnAuthorized("Token.TokenInvalid", "Token 不合法");
    }
}