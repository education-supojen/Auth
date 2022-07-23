using Auth.Domain.Aggregates;
using Dapper.FluentColumnMapping;

namespace Auth.Infrastructure.Persistence.Dapper;

public static class MiniORMMapping
{
    public static void CreateMapping()
    {
        var mappings = new ColumnMappingCollection();
        
        mappings.RegisterType<User>()
            .MapProperty(x => x.EmailConfirmed).ToColumn("email_confirmed")
            .MapProperty(x => x.PasswordHash).ToColumn("password_Hash");
        
        mappings.RegisterWithDapper();
    }
}