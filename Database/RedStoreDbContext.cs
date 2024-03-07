using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RedStore.Contracts;
using RedStore.Database.DomainModels;

namespace RedStore.Database;

public class RedStoreDbContext : DbContext
{

    public RedStoreDbContext(DbContextOptions dbContextOptions)
    :base(dbContextOptions) {}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder
            .Entity<Product>()
            .ToTable("products", t => t.ExcludeFromMigrations());

        modelBuilder
            .Entity<Category>()
            .ToTable("categories", t => t.ExcludeFromMigrations());


        modelBuilder
            .Entity<ProductColor>()
            .ToTable("ProductColors")
            .HasKey(x => new { x.ProductId, x.ColorId });


        modelBuilder
         .Entity<Color>()
         .HasData(

            new Color
            {
                Id = -1,
                Name = "Red"
            },
            new Color
            {
                Id = -2,
                Name = "Green"
            },
             new Color
             {
                 Id = -3,
                 Name = "Blue"
             },
              new Color
              {
                  Id = -4,
                  Name = "Black"
              }
            );


        base.OnModelCreating(modelBuilder);
    }


    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<ProductColor> ProductColors { get; set; }
}
