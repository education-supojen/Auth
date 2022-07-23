namespace Auth.Domain.Errors;

public static partial class Errors
{
    public static class Registration
    {
        public static Error VerificationUnCorrect => 
            Error.BadRequest("Registration.VerificationUnCorrect","驗證失敗");
        
        public static Error TooManyTimes => 
            Error.BadRequest("Registration.TooManyTimes","驗證太多次, 過段時間再重新註冊吧!");

        public static Error DoFirstStepFirst =>
            Error.BadRequest("Registration.DoFirstStepFirst", "請先完成註冊第一步");

    }
}