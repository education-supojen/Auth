namespace Auth.Domain.Errors;

public static partial class Errors
{
    public static class Company
    {
        public static Error ManagerIsNotExist => Error.BadRequest("Company.ManagerIsNotExist","主管不存在");
    }
}