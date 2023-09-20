using DoctorService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorService.Data.Configs
{
    public class DoctorConfig : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors", AppDbContext.Schema);
            builder.Property(p => p.FullName).HasMaxLength(228);
            builder.Property(p => p.UserId).HasMaxLength(36);
            builder.Property(p => p.SpecialityId).HasMaxLength(36);
            builder.HasOne(p => p.Speciality);
            builder.HasOne(p => p.User);

        }
    }
}
