using Auth.Domain.Aggregates;
using Auth.Domain.Repositories;
using Auth.Infrastructure.Persistence.EF;
using Auth.Infrastructure.Persistence.Redis;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly DbSet<User> _users;


    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _users = appDbContext.User;
    }
    
    /// <summary>
    /// 註冊最後一步, 新增使用者
    /// </summary>
    /// <param name="user"></param>
    public async Task AddAsync(User user)
    {
        await _users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 更新使用者
    /// </summary>
    /// <param name="user"></param>
    public async Task UpdateAsync(User user)
    {
        _users.Update(user);
        await _appDbContext.SaveChangesAsync();
    }
    
    /// <summary>
    /// 取得使用者
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<User?> GetAsync(long id)
    {
        return await _users.SingleOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// 使用 Email 找到以註冊的使用者
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _users.SingleOrDefaultAsync(x => x.Email == email);
    }
}