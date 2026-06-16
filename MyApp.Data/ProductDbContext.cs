using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class ProductDbContext : DbContext
{
    public DbSet<ProductEntity> Products { get; set; }
    
    public ProductDbContext(DbContextOptions<ProductDbContext> options): base(options)
    {
  
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}