using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Aggregates;
using Auth.Domain.Errors;
using Auth.Domain.Repositories;
using Auth.Infrastructure.Persistence.EF;
using Auth.Infrastructure.Persistence.Redis;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using OneOf;

namespace Auth.Infrastructure.Persistence.Repositories;

public class UnitOfWork: IUnitOfWork, IDisposable
{
    private readonly AppDbContext _appDbContext;        // Description - 管理 Database 連線
    private readonly IAggregateCache _aggregateCache;   // Description - 管理 Redis 連線, 和參與 Redis 處存的 Aggregates
    private readonly IMediator _mediator;               // Description - CQRS Mediator
    private IDbContextTransaction _transaction;         // Description - Database Transaction,如果決定自己管理 Transaction 會用到
    

    public UnitOfWork(
        IUserRepository userRepository,
        IStaffRepository staffRepository,
        IRegistrationRepository registrationRepository,
        IPasswordUpdateRepository passwordUpdateRepository,
        AppDbContext appDbContext,
        IAggregateCache aggregateCache,
        IMediator mediator)
    {
        UserRepository = userRepository;
        StaffRepository = staffRepository;
        RegistrationRepository = registrationRepository;
        PasswordUpdateRepository = passwordUpdateRepository;
        _appDbContext = appDbContext;
        _aggregateCache = aggregateCache;
        _mediator = mediator;
    }

    #region Repository Interface
    
    /// <summary>
    /// 使用者 - Repository
    /// </summary>
    public IUserRepository UserRepository { get; }

    /// <summary>
    /// 後台人員 - Repository
    /// </summary>
    public IStaffRepository StaffRepository { get; }

    /// <summary>
    /// 註冊 - Repo
    /// </summary>
    public IRegistrationRepository RegistrationRepository { get; }

    /// <summary>
    /// 更新密碼 - Repo
    /// </summary>
    public IPasswordUpdateRepository PasswordUpdateRepository { get; }

    #endregion
    
    
    #region About Domian Event
    
    /// <summary>
    /// 取得所有 Domain Events 並且交給指定的 Handler 去做處理
    /// </summary>
    private async Task<OneOf<bool,Failure>> DispatchDomainEventAsync()
    {
        // Processing -
        //     Collecting the Aggregate which has been modified or add
        //     Redis 實作的 Persistence 和 Ef Core 實作 Persistence 燈都要考慮到
        var aggregates = new List<AggregateRoot>();
        aggregates.AddRange(_aggregateCache.GetAggregates());
        aggregates.AddRange(_appDbContext.ChangeTracker.Entries<AggregateRoot>().Select(x => x.Entity));

        // Processing - 
        //     Collecting the domain events, 把所也發生過的 Domain Events 收集起來
        //     並且從 Aggregates 裡面清掉
        var events = new List<IRequest<OneOf<bool,Failure>>>();
        foreach (var aggregate in aggregates)
        {
            events.AddRange(aggregate.ClearEvents());
        }

        // Processing - 
        //     呼叫 Handler 處理掉所有的領域事件
        foreach (var @event in events)
        {
            var task = await _mediator.Send(@event);
            if (task.IsT1) return task;
        }

        // Mission Complete
        return true;
    }
    
    #endregion
    
    #region IUnitOfWork Interface

    /// <summary>
    /// Begin Transaction (Async)
    /// </summary>
    public async Task BeginTransactionAsync()
    {
        if (_transaction != null) return;
        _transaction = await _appDbContext.Database.BeginTransactionAsync();
    }
    
    
    /// <summary>
    /// 處理 Domain Events 並且建立 Savepoint
    /// </summary>
    public async Task<OneOf<bool,Failure>> SaveAggregatesAsync()
    {
        // Processing - 處理 Domain Events
        var task = await DispatchDomainEventAsync();

        // Processing - 如果處理 Domain 的過程發生錯誤, 返回錯誤
        if (task.IsT1) return task;
        
        // Processing - 自動產生 Savepoint (在沒有手動起 Transaction 的情況下), 或是隱性的 commit transaction。
        await _appDbContext.SaveChangesAsync();

        // Mission Complete
        return true;
    }
    
    
    /// <summary>
    /// Commit Transaction (Async)
    /// </summary>
    /// <returns></returns>
    public async Task<OneOf<bool,Failure>> CommitAsync() 
    {
        // Processing - 檢查是不是有手動起過 Transaction, 沒有的話不給手動 Commit
        if (_transaction == null) throw new ArgumentNullException(nameof(_transaction));
        
        // Processing - 處理 Domain Events 並且建立 Savepoint
        var task = await SaveAggregatesAsync();

        // Processing Domain Events 處理發生錯誤, 就返回錯誤結果
        if (task.IsT1) return task;
        
        // Processing -
        //     Commit the change, end the transaction, 事件都處理完後, 最後才能結束 transaction, 
        //     到這一步, Sql Database 才會真正被更改
        await _transaction.CommitAsync();

        // Mission Complete
        return true;
    }
    


    #endregion
    
    
    #region 資源釋放
    private bool _disposed = false;
    protected virtual void Dispose(bool disposing) 
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _appDbContext.Dispose();
            }
        }
        _disposed = true;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}