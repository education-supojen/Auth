namespace Auth.Application.Interfaces.Persistence;

public interface IMiniORMDatabase
{
    /// <summary>
    /// 搜尋多筆
    /// </summary>
    /// <param name="sql"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<IEnumerable<T>> QueryAsync<T>(string sql);

    /// <summary>
    /// 搜尋一比
    /// </summary>
    /// <param name="sql"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T?> FindAsync<T>(string sql);
     
    /// <summary>
    /// 執行運算
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    Task<int> ExecuteAsync(string sql);
    
    /// <summary>
    /// Begin Transaction
    /// </summary>
    void BeginTransaction();

    /// <summary>
    /// Begin Transaction (Async)
    /// </summary>
    Task BeginTransactionAsync();

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