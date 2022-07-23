using Auth.Domain.Aggregates;
using Auth.Domain.Repositories;
using Auth.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Persistence;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Company> _companies;

    public CompanyRepository(AppDbContext context)
    {
        _context = context;
        _companies = _context.Company;
    }
    
    public async Task<Company> GetAsync(long id)
    {
        return await _companies.FindAsync(id);
    }

    public async Task AddAsync(Company company)
    {
        await _companies.AddAsync(company);
        await _context.SaveChangesAsync();
    }
}