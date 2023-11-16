using HistoryService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HistoryService.Data.Configs
{
    public class DoctorConfig : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors", AppDbContext.Schema);
            builder.Property(p => p.FullName).HasMaxLength(228);
            builder.Property(p => p.Speciality).HasMaxLength(128);
            builder.Property(p => p.ExternalId).HasMaxLength(36);
        }
    }
}
