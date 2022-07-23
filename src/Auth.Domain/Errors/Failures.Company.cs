namespace Auth.Domain.Errors;

public static partial class Failures
{
    public static class Company
    {
        public static Failure ManagerIsNotExist => Failure.BadRequest("Company.ManagerIsNotExist","主管不存在");
    }
}