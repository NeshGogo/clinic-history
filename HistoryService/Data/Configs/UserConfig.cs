using HistoryService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HistoryService.Data.Configs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", AppDbContext.Schema);
            builder.Property(p => p.FullName).HasMaxLength(256);
            builder.Property(p => p.Email).HasMaxLength(256);
            builder.Property(p => p.ExternalId).HasMaxLength(36);
        }
    }
}
