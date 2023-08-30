using AccountService.Data;
using AccountService.Profiles;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AccountServiceTest
{
    public class BaseTest
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
                options.AddProfile(new AutoMapperProfile());
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

    }
}
