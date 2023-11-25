using HistoryService.Entities;
using HistoryService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

namespace HistoryService.Data
{
    public static class PreDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                var grpcUserClient = serviceScope.ServiceProvider.GetService<IUserDataClient>();
                var grpcDoctorClient = serviceScope.ServiceProvider.GetService<IDoctorDataClient>();
                
                var users = grpcUserClient.ReturnsAllUsers();
                var doctors = grpcDoctorClient.ReturnsAllDoctors();
                
                SeedData(context, isProd, users, doctors);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd, IEnumerable<User> users, IEnumerable<Doctor> doctors)
        {
            SeedEntityData(context, users);
            SeedEntityData(context, doctors);

            if (isProd)
            {
                try
                {
                    Console.WriteLine("--> Attempting to apply migrations...");
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations. Error: {ex.Message}");
                }
            }
        }

        private static void SeedEntityData<T>(AppDbContext context, IEnumerable<T> entities) where T : BaseEntity, IExternalId
        {
            try
            {
                Console.WriteLine($"--> Seeding new {typeof(T).Name}...");
                foreach (var entity in entities)
                {
                    if (context.Set<T>().Any(p => p.ExternalId == entity.ExternalId))
                    {
                        entity.Create("System");
                        context.Add(entity);
                    }  
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not seeding new {typeof(T).Name} becase of error: {ex.Message}");
            }
        }
    }
}
