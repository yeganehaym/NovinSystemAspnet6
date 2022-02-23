using Microsoft.EntityFrameworkCore;
using WebApplication2.Data.Domains;

namespace WebApplication2.Data;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions options):base(options)
    {
        
    }
    
    public DbSet<Product> Products { get; set; }
}