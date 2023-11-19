using HistoryService.Data.Configs;
using HistoryService.Entities;
using Microsoft.EntityFrameworkCore;

namespace HistoryService.Data
{
    public class AppDbContext : DbContext
    {
        public const string Schema = "HistoryService";
        public AppDbContext(DbContextOptions opt) : base(opt)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new UserConfig().Configure(modelBuilder.Entity<User>());
            new DoctorConfig().Configure(modelBuilder.Entity<Doctor>());
            new PatientConfig().Configure(modelBuilder.Entity<Patient>());
            new ClinicRecordConfig().Configure(modelBuilder.Entity<ClinicRecord>());
        }
    }
}
