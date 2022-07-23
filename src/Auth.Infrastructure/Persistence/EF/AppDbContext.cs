using System.Reflection.PortableExecutable;
using Auth.Domain.Aggregates;
using Auth.Domain.Entities;
using Auth.Domain.Errors;
using Auth.Domain.Factories;
using Auth.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Persistence.EF;

public class AppDbContext : DbContext
{
    public DbSet<User> User { get; set; }

    public DbSet<Staff> Staff { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var configuration = new DbConfiguration();
        modelBuilder.ApplyConfiguration<User>(configuration);
        modelBuilder.ApplyConfiguration<Staff>(configuration);
    }
}