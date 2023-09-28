using AutoMapper;
using DoctorService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DoctorService.Profiles;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Test
{
    public class TestBase
    {
        protected readonly string defaultUserId = "52736a28-633f-496c-9c2e-3d1fb986a9fd";
        protected readonly string defaultUserEmail = "userTest@test.com";

        protected AppDbContext BuildContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName).Options;
            return new AppDbContext(options);
        }

        protected IMapper ConfigureAutoMapper()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutomapperProfiles());
            });
            return config.CreateMapper();
        }

        protected ControllerContext BuildControllerContext()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, defaultUserEmail),
                new Claim(ClaimTypes.Name, defaultUserEmail),
                new Claim(ClaimTypes.NameIdentifier, defaultUserId),
            }));

            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }
        protected HttpContext BuildHttpContext()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, defaultUserEmail),
                new Claim(ClaimTypes.Name, defaultUserEmail),
                new Claim(ClaimTypes.NameIdentifier, defaultUserId),
            }));

            return new DefaultHttpContext { User = user };           
        }
    }
}
