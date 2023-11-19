using HistoryService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HistoryService.Data.Configs
{
    public class ClinicRecordConfig : IEntityTypeConfiguration<ClinicRecord>
    {
        public void Configure(EntityTypeBuilder<ClinicRecord> builder)
        {
            builder.ToTable("ClinicRecords", AppDbContext.Schema);
            builder.Property(p => p.Diagnosis).HasMaxLength(500);
            builder.Property(p => p.PatientId).HasMaxLength(36);
            builder.Property(p => p.DoctorId).HasMaxLength(36);
            builder.HasOne(p => p.Doctor);
            builder.HasOne(p => p.Patient);
        }
    }
}
