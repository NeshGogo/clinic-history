using AccountService.AsyncDataService;
using AccountService.Entities;
using AccountService.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Data
{
    public static class PrepDb
    {
        public static void PrepPoupulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                var messageBusClient = serviceScope.ServiceProvider.GetService<IMessageBusClient>();
                SeedData(context,userManager, messageBusClient, isProd);
            }
        }

        private static void SeedData(AppDbContext context, UserManager<User> userManager, IMessageBusClient messageBusClient, bool isProd)
        {
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

            if (!context.Users.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                var user = new User
                {
                    Email = "neshgogo@test.com",
                    FullName = "Nesh Gogo",
                    Identification = "12345678912",
                    UserType = "Admin",
                    UserName = "neshgogo@test.com",
                    RecordCreatedBy = "neshgogo@test.com",
                    RecordUpdatedBy = "neshgogo@test.com",
                };
                var result = userManager.CreateAsync(user, "badPassowrd12!3").Result;
                if (!result.Succeeded)
                {
                    Console.WriteLine("--> Could not create default user");
                }
                else
                {
                    Console.WriteLine("--> Default user created");
                    messageBusClient.PublishNewUser(new DTOs.UserPublishDTO
                    {
                        Email = user.Email,
                        FullName = user.FullName,
                        Id = user.Id,
                        Event = MessageBusEventType.NewUser,
                    });
                }
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
