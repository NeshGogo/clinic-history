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
        }
    }
}
