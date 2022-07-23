using Auth.Domain.Aggregates;

namespace Auth.Domain.Repositories;

public interface ICompanyRepository
{
    /// <summary>
    /// 取得 - 公司
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Company> GetAsync(long id);
    
    /// <summary>
    /// 新增 - 公司
    /// </summary>
    /// <param name="company"></param>
    /// <returns></returns>
    Task AddAsync(Company company);
}