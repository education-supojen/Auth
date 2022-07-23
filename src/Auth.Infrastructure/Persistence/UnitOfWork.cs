using System.Data.Common;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Repositories;
using Auth.Infrastructure.Persistence.EF;
using Npgsql;

namespace Auth.Infrastructure.Persistence;

public class UnitOfWork: IUnitOfWork, IDisposable
{
    private readonly AppDbContext _appDbContext;

    public UnitOfWork(
        IUserRepository userRepository,
        ICompanyRepository companyRepository,
        AppDbContext appDbContext)
    {
        UserRepository = userRepository;
        CompanyRepository = companyRepository;
        _appDbContext = appDbContext;
    }

    #region Repository Interface

    /// <summary>
    /// 使用者 - Repo
    /// </summary>
    public IUserRepository UserRepository { get; }

    /// <summary>
    /// 公司 - Repo
    /// </summary>
    public ICompanyRepository CompanyRepository { get; }

    #endregion
    
    #region IUnitOfWork Interface

    /// <summary>
    /// Begin Transaction
    /// </summary>
    public void BeginTransaction()
    {
        _appDbContext.Database.BeginTransaction();
    }

    /// <summary>
    /// Begin Transaction (Async)
    /// </summary>
    public async Task BeginTransactionAsync()
    {
        await _appDbContext.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Commit Transaction
    /// </summary>
    /// <returns></returns>
    public void Commit()
    {
        _appDbContext.Database.CommitTransaction();
    }

    /// <summary>
    /// Commit Transaction (Async)
    /// </summary>
    /// <returns></returns>
    public async Task CommitAsync()
    {
        await _appDbContext.Database.CommitTransactionAsync();
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
                _appDbContext.Dispose();
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