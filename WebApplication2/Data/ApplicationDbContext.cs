using Microsoft.EntityFrameworkCore;
using WebApplication2.Data.Config;
using WebApplication2.Data.Domains;
using WebApplication2.Data.Entity;

namespace WebApplication2.Data;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions options):base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.Entity<User>().Property(x => x.FirstName).HasMaxLength(100);
        modelBuilder.Entity<User>().Property(x => x.LastName).HasMaxLength(100);
        modelBuilder.Entity<User>().Property(x => x.Username).HasMaxLength(15);
        modelBuilder.Entity<User>().Property(x => x.Password).HasMaxLength(200);
        modelBuilder.Entity<User>().Property(x => x.Email).HasMaxLength(200);
        modelBuilder.Entity<User>().Property(x => x.MobileNumber).HasMaxLength(11);*/
        modelBuilder.Entity<OtpCode>().Property(x => x.Code).HasMaxLength(6);
        modelBuilder.Entity<OtpCode>().HasIndex(x => x.Code).IsUnique();
        
        modelBuilder.ApplyConfiguration(new UserConfig());
        modelBuilder.ApplyConfiguration(new CustomerConfig());
        modelBuilder.ApplyConfiguration(new InvoiceItemConfig());
        modelBuilder.ApplyConfiguration(new ProductConfig());
        
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<OtpCode> OtpCodes { get; set; }
    public DbSet<ProductService> ProductServices { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItem { get; set; }
    public DbSet<Customer> Customers { get; set; }
}