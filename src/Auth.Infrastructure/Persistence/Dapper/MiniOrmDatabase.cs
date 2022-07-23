using Auth.Domain.Repositories;
using Npgsql;

namespace Auth.Infrastructure.Persistence.Dapper;

public class MiniOrmDatabase : IMiniORMDatabase
{
    /// <summary>
    /// Database 連線
    /// </summary>
    public NpgsqlConnection Connection { get; }
    
    /// <summary>
    /// Transaction
    /// </summary>
    public NpgsqlTransaction Transaction { get; private set; }

    public MiniOrmDatabase(NpgsqlConnection connection)
    {
        Connection = connection;
    }

    #region IUnitOfWork Interface

    /// <summary>
    /// Begin Transaction
    /// </summary>
    public void Begin()
    {
        //this._ctx.Database.BeginTransaction();
        Transaction = Connection.BeginTransaction();
    }

    /// <summary>
    /// Begin Transaction (Async)
    /// </summary>
    public async Task BeginAsync()
    {
        Transaction = await Connection.BeginTransactionAsync();
    }

    /// <summary>
    /// Commit Transaction
    /// </summary>
    /// <returns></returns>
    public bool Commit()
    {
        try
        {
            Transaction.Commit();
            return true;
        }
        catch (Exception)
        {
            Transaction.Rollback();
            return false;
        }
    }

    /// <summary>
    /// Commit Transaction (Async)
    /// </summary>
    /// <returns></returns>
    public async Task<bool> CommitAsync()
    {
        try
        {
            await Transaction.CommitAsync();
            return true;
        }
        catch (Exception e)
        {
            await Transaction.RollbackAsync();
            return false;
        }
    }

    #endregion
    
    
    #region 資源釋放
    private bool _disposed = false;
    protected virtual void Dispose(bool disposing) 
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                if(Transaction != null) 
                    Transaction.Dispose();
                Connection.Dispose();
            }
        }
        this._disposed = true;
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}