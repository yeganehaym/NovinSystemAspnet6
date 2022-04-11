using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication2.Data.Entity;

namespace WebApplication2.Data.Config;

public class UserConfig:IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.ImageProfile).HasMaxLength(100);
        builder.Property(x => x.SerialNo).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);
        builder.Property(x => x.Password).HasMaxLength(100);
        builder.Property(x => x.MobileNumber).HasMaxLength(11);
        builder.Property(x => x.Username).HasMaxLength(100);
    }
}