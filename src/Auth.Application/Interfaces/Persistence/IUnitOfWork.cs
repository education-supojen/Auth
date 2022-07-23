using Auth.Domain.Repositories;

namespace Auth.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    /// <summary>
    /// 使用者 - Repo
    /// </summary>
    IUserRepository UserRepository { get; }
    
    /// <summary>
    /// 公司 - Repo
    /// </summary>
    ICompanyRepository CompanyRepository { get; }

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
    void Commit();

    /// <summary>
    /// Commit Transaction (Async)
    /// </summary>
    /// <returns></returns>
    Task CommitAsync();
}