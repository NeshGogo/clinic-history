using DoctorService.Data.Configs;
using DoctorService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Data
{
    public class AppDbContext : DbContext
    {
        public const string Schema = "DoctorService";
        public AppDbContext(DbContextOptions options) : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new SpecialityConfig().Configure(modelBuilder.Entity<Speciality>());
            new DoctorConfig().Configure(modelBuilder.Entity<Doctor>());
            new UserConfig().Configure(modelBuilder.Entity<User>());
        }
    }
}
