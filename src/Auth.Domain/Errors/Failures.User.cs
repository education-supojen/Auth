namespace Auth.Domain.Errors;

public static partial class Failures
{
    public static class User
    {
        public static Failure DuplicateEmail => Failure.BadRequest("User.DuplicateEmail","郵箱已經被人使用過了");

        public static Failure PasswordWrong => Failure.BadRequest("User.PasswordWrong", "使用者密碼不正確");
        
        public static Failure Null => Failure.BadRequest("User.Null", "使用者不存在");

        public static Failure DataCorruption => Failure.BadRequest("User.DataCorruption", "資料損毀");
    }
}