using DoctorService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorService.Data.Configs
{
    public class SpecialityConfig : IEntityTypeConfiguration<Speciality>
    {
        public void Configure(EntityTypeBuilder<Speciality> builder)
        {
            builder.ToTable("Specialities", AppDbContext.Schema);
            builder.Property(p => p.Name).HasMaxLength(128);
            builder.Property(p => p.Description).HasMaxLength(800);
        }
    }
}
