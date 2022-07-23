using Auth.Domain.Errors;
using Auth.Domain.Repositories;
using OneOf;

namespace Auth.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    /// <summary>
    /// 使用者 - Repo
    /// </summary>
    IUserRepository UserRepository { get; }
    
    /// <summary>
    /// 後台人員 - Repository
    /// </summary>
    IStaffRepository StaffRepository { get; }

    /// <summary>
    /// 註冊 - Repo
    /// </summary>
    IRegistrationRepository RegistrationRepository { get; }

    /// <summary>
    /// 更新密碼 - Repo
    /// </summary>
    IPasswordUpdateRepository PasswordUpdateRepository { get; }

    /// <summary>
    /// Automatically Commit Transaction
    /// </summary>
    Task<OneOf<bool,Failure>> SaveAggregatesAsync();
    
    /// <summary>
    /// Begin Transaction (Async)
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commit Transaction (Async)
    /// </summary>
    /// <returns></returns>
    Task<OneOf<bool,Failure>> CommitAsync();
}