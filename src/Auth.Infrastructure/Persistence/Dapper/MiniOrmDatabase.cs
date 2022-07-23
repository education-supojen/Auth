using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Repositories;
using Dapper;
using Npgsql;

namespace Auth.Infrastructure.Persistence.Dapper;

public class MiniORMDatabase : IMiniORMDatabase
{
    /// <summary>
    /// Database 連線
    /// </summary>
    protected NpgsqlConnection Connection { get; }
    
    /// <summary>
    /// Transaction
    /// </summary>
    protected NpgsqlTransaction Transaction { get; private set; }

    public MiniORMDatabase(NpgsqlConnection connection)
    {
        Connection = connection;
        Connection.Open();
    }

    #region IUnitOfWork Interface

    /// <summary>
    /// 搜尋多筆
    /// </summary>
    /// <param name="sql"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<IEnumerable<T>> QueryAsync<T>(string sql)
    {
        return await Connection.QueryAsync<T>(sql);
    }

    /// <summary>
    /// 搜尋一比
    /// </summary>
    /// <param name="sql"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T?> FindAsync<T>(string sql)
    {
        return await Connection.QueryFirstOrDefaultAsync<T>(sql);
    }

    /// <summary>
    /// 執行運算
    /// </summary>
    /// <param name="sql"></param>
    public async Task<int> ExecuteAsync(string sql)
    {
        return await Connection.ExecuteAsync(sql,transaction:Transaction);
    }

    /// <summary>
    /// Begin Transaction
    /// </summary>
    public void BeginTransaction()
    {
        //this._ctx.Database.BeginTransaction();
        Transaction = Connection.BeginTransaction();
    }

    /// <summary>
    /// Begin Transaction (Async)
    /// </summary>
    public async Task BeginTransactionAsync()
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