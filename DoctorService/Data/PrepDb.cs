using DoctorService.Entities;
using DoctorService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Data
{
    public static class PrepDb
    {
        public static void PrepPoupulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IUserDataClient>();
                var users = grpcClient.ReturnsAllUsers() ?? new List<User>();
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                SeedData(context,isProd, users);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd, IEnumerable<User> users)
        {
            try
            {
                Console.WriteLine("--> Seeding new users...");
                foreach (var user in users)
                {
                    if (!context.Set<User>().Any(p => p.ExternalId == user.ExternalId))
                    {
                        user.Create("System");
                        context.Add(user);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not seeding new users becase of error: {ex.Message}");
            }

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
    }
}
