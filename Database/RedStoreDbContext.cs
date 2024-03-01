using Microsoft.EntityFrameworkCore;
using RedStore.Contracts;
using RedStore.Database.DomainModels;

namespace RedStore.Database;

public class RedStoreDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var factory = LoggerFactory.Create(builder => { builder.AddConsole(); });
            
        optionsBuilder
            .UseLoggerFactory(factory)
            .UseNpgsql(DatabaseConstants.CONNECTION_STRING);
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}
