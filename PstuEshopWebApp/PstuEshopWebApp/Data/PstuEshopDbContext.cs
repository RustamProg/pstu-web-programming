using Microsoft.EntityFrameworkCore;

namespace PstuEshopWebApp.Data;

public class PstuEshopDbContext(DbContextOptions<PstuEshopDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}
