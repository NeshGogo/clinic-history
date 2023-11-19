using HistoryService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HistoryService.Data.Configs
{
    public class PatientConfig : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patients", AppDbContext.Schema);
            builder.Property(p => p.FullName).HasMaxLength(128);
            builder.Property(p => p.Identification).HasMaxLength(11);
            builder.Property(p => p.Sex).HasMaxLength(20);
            builder.Property(p => p.UserId).HasMaxLength(36);
            builder.HasOne(p => p.User);
        }
    }
}
