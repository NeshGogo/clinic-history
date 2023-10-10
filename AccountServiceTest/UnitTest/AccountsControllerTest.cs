using AccountService.Controllers;
using AccountService.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountService.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AccountService.Services;
using AccountService.Data.Repositories;
using System.Xml.Linq;
using AccountService.AsyncDataService;

namespace AccountServiceTest.UnitTest
{
    public class AccountsControllerTest : BaseTest
    {
        private string _dbName;
        private Mock<IJwtService> jwtServiceMock;
        private Mock<IMessageBusClient> messageBusMock;

        [SetUp]
        public async Task Setup()
        {
            _dbName = Guid.NewGuid().ToString();
            jwtServiceMock = new Mock<IJwtService>();
            messageBusMock = new Mock<IMessageBusClient>();
        }

        [Test]
        public void ShouldGetUserTypes()
        {
            var controller = BuildAccountsController(_dbName);
            var results = controller.GetUserTypes();
            Assert.AreEqual(3, results.Value.Count);
        }

        [Test]
        public async Task ShouldCreateAnUser()
        {
            await CreateUser(_dbName);
            var context2 = BuildContext(_dbName);
            var count = await context2.Users.CountAsync();
            var users = context2.Users.ToList();
            Assert.AreEqual(1, count);
        }

        [Test]
        public async Task ShouldNotCreateUserBecauseUserTypeIsNotValid()
        {
            var controller = BuildAccountsController(_dbName);
            var user = new UserCreateDTO()
            {
                Email = "test@test.com",
                Password = "badPassowrd12!3",
                PasswordConfirm = "badPassowrd12!3",
                FullName = "Fulano Test",
                Identification = "12345678912",
                UserType = "Patientss"
            };
            var result = (await controller.CreateUser(user)).Result as BadRequestObjectResult;
            var context2 = BuildContext(_dbName);
            var count = await context2.Users.CountAsync();
            Assert.AreEqual(0, count);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status400BadRequest);
            Assert.AreEqual(result.Value, "UserType is not valid");
        }

        [Test]
        public async Task ShouldUpdateAnUser()
        {
            await CreateUser(_dbName);
            var controller = BuildAccountsController(_dbName);
            controller.ControllerContext = BuildControllerContext();
            var context = BuildContext(_dbName);
            var user = new UserCreateDTO()
            {
                Email = "test@test.com",
                Password = "badPassowrd12!3",
                PasswordConfirm = "badPassowrd12!3",
                FullName = "Fulano Test1",
                Identification = "12345678912",
                UserType = "Patient"
            };
            string id = context.Users.First().Id;
            var result = await controller.UpdateUser(id, user);
            Assert.AreEqual(result.Value.FullName, user.FullName);
            var context2 = BuildContext(_dbName);
            Assert.AreEqual(context2.Set<User>().First().FullName, user.FullName);
        }

        [Test]
        public async Task ShouldNotUpdateAnUserBecauseIdNotFound()
        {
            await CreateUser(_dbName);
            var controller = BuildAccountsController(_dbName);
            var user = new UserCreateDTO()
            {
                Email = "test@test.com",
                Password = "badPassowrd12!3",
                PasswordConfirm = "badPassowrd12!3",
                FullName = "Fulano Test1",
                Identification = "12345678912",
                UserType = "Patient"
            };
            string id = "1";
            var result = (await controller.UpdateUser(id, user)).Result as NotFoundObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status404NotFound);
            Assert.AreEqual(result.Value, $"Could not find the user with ID {id}");
        }

        [Test]
        public async Task ShouldDeleteAnUser()
        {
            await CreateUser(_dbName);
            var controller = BuildAccountsController(_dbName);
            var context = BuildContext(_dbName);
            var id = context.Users.First().Id;
            var result = await controller.DeleteUser(id) as NoContentResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status204NoContent);
        }

        [Test]
        public async Task ShouldNotDeleteAnUserBecauseNotFound()
        {
            await CreateUser(_dbName);
            var controller = BuildAccountsController(_dbName);
            var id = "id12";
            var result = await controller.DeleteUser(id) as NotFoundObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status404NotFound);
        }

        private async Task CreateUser(string dbName)
        {
            var controller = BuildAccountsController(dbName);
            var user = new UserCreateDTO()
            {
                Email = "test@test.com",
                Password = "badPassowrd12!3",
                PasswordConfirm = "badPassowrd12!3",
                FullName = "Fulano Test",
                Identification = "12345678912",
                UserType = "Patient"
            };
            await controller.CreateUser(user);
        }

        private AccountsController BuildAccountsController(string dbName)
        {
            var context = BuildContext(dbName);
            var myUserStore = new UserStore<User>(context);
            var userManager = BuildUserManager<User>(myUserStore);
            var userRepository = new UserRepository(context);
            var mapper = ConfigureAutoMapper();
            var httpContext = new DefaultHttpContext();
            MockAuth(httpContext);
            var signInManager = SetupSignInManager(userManager, httpContext);
            var myConfiguration = new Dictionary<string, string>
            {
                {"jwt:key", "Klw449hzuEkbKyJMdjlOeDmudu7XjCBiV8IVb3COLDwT7u1cbsANDocOID2A037QXYkWhhP6qzEuF9cDbLPhQUxSK6m1AGtA6WAw" }
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
            return new AccountsController(userManager, mapper, jwtServiceMock.Object, userRepository, messageBusMock.Object);
        }

        private UserManager<TUser> BuildUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
        {
            store = store ?? new Mock<IUserStore<TUser>>().Object;
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;

            options.Setup(o => o.Value).Returns(idOptions);

            var userValidators = new List<IUserValidator<TUser>>();

            var validator = new Mock<IUserValidator<TUser>>();
            userValidators.Add(validator.Object);
            var pwdValidators = new List<PasswordValidator<TUser>>();
            pwdValidators.Add(new PasswordValidator<TUser>());

            var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<TUser>>>().Object);

            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();

            return userManager;
        }

        private Mock<IAuthenticationService> MockAuth(HttpContext context)
        {
            var auth = new Mock<IAuthenticationService>();
            context.RequestServices = new ServiceCollection().AddSingleton(auth.Object).BuildServiceProvider();
            return auth;
        }

        private static SignInManager<TUser> SetupSignInManager<TUser>(UserManager<TUser> manager,
           HttpContext context, ILogger logger = null, IdentityOptions identityOptions = null,
           IAuthenticationSchemeProvider schemeProvider = null) where TUser : class
        {
            var contextAccessor = new Mock<IHttpContextAccessor>();
            contextAccessor.Setup(a => a.HttpContext).Returns(context);
            identityOptions = identityOptions ?? new IdentityOptions();
            var options = new Mock<IOptions<IdentityOptions>>();
            options.Setup(a => a.Value).Returns(identityOptions);
            var claimsFactory = new UserClaimsPrincipalFactory<TUser>(manager, options.Object);
            schemeProvider = schemeProvider ?? new Mock<IAuthenticationSchemeProvider>().Object;
            var sm = new SignInManager<TUser>(manager, contextAccessor.Object, claimsFactory, options.Object, null, schemeProvider, new DefaultUserConfirmation<TUser>());
            sm.Logger = logger ?? (new Mock<ILogger<SignInManager<TUser>>>()).Object;
            return sm;
        }

    }
}
