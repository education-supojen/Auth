using Npgsql;

namespace Auth.Infrastructure.Persistence.Dapper;

public interface IMiniORMDatabase
{
    /// <summary>
    /// Database 連線
    /// </summary>
    NpgsqlConnection Connection { get; }
    
    /// <summary>
    /// Transaction
    /// </summary>
    NpgsqlTransaction Transaction { get; }

    /// <summary>
    /// Begin Transaction
    /// </summary>
    void Begin();

    /// <summary>
    /// Begin Transaction (Async)
    /// </summary>
    Task BeginAsync();

    /// <summary>
    /// Commit Transaction
    /// </summary>
    /// <returns></returns>
    bool Commit();

    /// <summary>
    /// Commit Transaction (Async)
    /// </summary>
    /// <returns></returns>
    Task<bool> CommitAsync();
}