using System.Security.AccessControl;

namespace Auth.Domain.Errors;

public static partial class Failures
{
    public static class Update
    {
        public static Failure Null => Failure.UnAuthorized("Update.Null", "請回上一步");

        public static Failure TokenInvalid => Failure.UnAuthorized("Update.TokenInvalid", "Token 不合法");
    }
}