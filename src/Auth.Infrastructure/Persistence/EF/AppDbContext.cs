using System.Reflection.PortableExecutable;
using Auth.Domain.Aggregates;
using Auth.Domain.Errors;
using Auth.Domain.Factories;
using Auth.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Persistence.EF;

public class AppDbContext : DbContext
{
    public DbSet<User> User { get; set; }
    
    public DbSet<Company> Company { get; set; }

    public DbSet<Department> Department { get; set; }

    public DbSet<Schedule> Schedule { get; set; }

    public DbSet<Shift> Shift { get; set; }

    public DbSet<PunchGroup> PunchSetting { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var configuration = new DbConfiguration();
        modelBuilder.ApplyConfiguration<User>(configuration);
        modelBuilder.ApplyConfiguration<Company>(configuration);
        modelBuilder.ApplyConfiguration<Department>(configuration);
        modelBuilder.ApplyConfiguration<Schedule>(configuration);
        modelBuilder.ApplyConfiguration<Shift>(configuration);
        modelBuilder.ApplyConfiguration<PunchGroup>(configuration);
    }
}