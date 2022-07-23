namespace Auth.Domain.Errors;

public static partial class Failures
{
    public static class Registration
    {
        public static Failure TokenInvalid => Failure.BadRequest("Reg.TokenInvalid", "令牌不正確");
        
        public static Failure CodeUnCorrect => Failure.BadRequest("Reg.CodeUnCorrect","驗證失敗");
        
        public static Failure NoChance => Failure.BadRequest("Reg.NoChance","驗證太多次, 過段時間再重新註冊吧!");

        public static Failure Null => Failure.BadRequest("Reg.PreviousStep", "請回到第一步來");

    }
}