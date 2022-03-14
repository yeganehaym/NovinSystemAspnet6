using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication2.Data.Entity;

namespace WebApplication2.Data.Config;

public class ProductConfig:IEntityTypeConfiguration<ProductService>
{
    public void Configure(EntityTypeBuilder<ProductService> builder)
    {
        builder.HasOne(x => x.User)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.UserId);
    }
}