using Microsoft.EntityFrameworkCore;
using RedStore.Contracts;
using RedStore.Database.DomainModels;

namespace RedStore.Database;

public class RedStoreDbContext : DbContext
{

    public RedStoreDbContext(DbContextOptions dbContextOptions)
    :base(dbContextOptions) {}

    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}
