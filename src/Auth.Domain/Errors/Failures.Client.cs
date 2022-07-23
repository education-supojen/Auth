namespace Auth.Domain.Errors;

public static partial class Failures
{
    public static class Client
    {
        public static Failure IdError => Failure.BadRequest("Client.IdError","ID不正確");
        
        public static Failure SecretError => Failure.BadRequest("Client.SecretError","Secret不正確");
    }
}