namespace Auth.Domain.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail => Error.BadRequest("User.DuplicateEmail","郵箱已經被人使用過了");

        public static Error PasswordWrong => Error.BadRequest("User.PasswordWrong", "使用者密碼不正確");
        
        public static Error UserIsNotExist => Error.BadRequest("User.UserIsNotExist", "使用者不存在");
        
    }
}