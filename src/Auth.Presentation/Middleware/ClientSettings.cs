namespace Auth.Presentation.Middleware;

public class ClientSettings
{
    public const string SectionName = "ClientSettings";

    public Client[] Clients { get; set; }
}

public record Client(string Id, string Secret);