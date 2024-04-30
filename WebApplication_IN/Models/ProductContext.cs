using Microsoft.EntityFrameworkCore;

namespace WebApplication_IN.Models
{
    public class ProductContext(DbContextOptions<ProductContext> options) : DbContext(options)
    {
        public DbSet<Product> Product { get; set; } = null!;
    }
}
