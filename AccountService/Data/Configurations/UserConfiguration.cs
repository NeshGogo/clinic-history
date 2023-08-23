using AccountService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", AppDbContext.Schema);
            builder.Property(p => p.FullName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.UserType).IsRequired().HasMaxLength(20);
            builder.Property(p => p.Identification).IsRequired().HasMaxLength(11);
        }
    }
}
