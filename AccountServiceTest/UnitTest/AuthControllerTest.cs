using AccountService.AsyncDataService;
using AccountService.Controllers;
using AccountService.Data.Repositories;
using AccountService.DTOs;
using AccountService.Entities;
using AccountService.Services;
using AccountServiceTest;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http;
using System.Security.Claims;
using System.Xml.Linq;

namespace AccountService.Test.UnitTest
{
    public class AuthControllerTest : BaseTest
    {
        private string _dbName;
        private Mock<IJwtService> jwtServiceMock;
        private Mock<IMessageBusClient> messageBusMock;
        private DefaultHttpContext httpContext;
        private IConfigurationRoot configuration;

        [SetUp]
        public async Task Setup()
        {
            _dbName = Guid.NewGuid().ToString();
            jwtServiceMock = new Mock<IJwtService>();
            messageBusMock = new Mock<IMessageBusClient>();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, defaultUserEmail) }));
            httpContext = new DefaultHttpContext() { User = user };
            var myConfiguration = new Dictionary<string, string>
            {
                {"jwt:key", "Klw449hzuEkbKyJMdjlOeDmudu7XjCBiV8IVb3COLDwT7u1cbsANDocOID2A037QXYkWhhP6qzEuF9cDbLPhQUxSK6m1AGtA6WAw" }
            };
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
        }

        [Test]
        public async Task ShouldLogin()
        {
            await CreateUser(_dbName);
            var user = new UserInfoDTO { Email = "test@test.com", Password = "badPassowrd12!3" };
            jwtServiceMock.Setup(p => p.BuildToken(user)).Returns(Task.FromResult(new UserTokenDTO { Token = "123213" }));
            var controller = BuildAuthController();            
            var result = await controller.Login(user);;
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(result.Value.Token, "123213");
        }

        [Test]
        public async Task ShouldNotLoginInvalidUserOrPassword()
        {
            await CreateUser(_dbName);
            var user = new UserInfoDTO { Email = "test@test.com", Password = "badPassowrd12!3321" };
            var controller = BuildAuthController();
            var result = (await controller.Login(user)).Result as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status400BadRequest);
            Assert.AreEqual(result.Value, "Invalid login attempt");
        }

        [Test]
        public async Task ShouldRenewToken()
        {
            await CreateUser(_dbName);
            jwtServiceMock.Setup(p => p.BuildToken(It.IsAny<UserInfoDTO>())).ReturnsAsync(new UserTokenDTO { Expiration = DateTime.Now, Token = "123213" });
            var controller = BuildAuthController();
            controller.ControllerContext.HttpContext = httpContext;
            var result = await controller.Renovate();
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(result.Value.Token, "123213");
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


        private AuthController BuildAuthController()
        {
            var context = BuildContext(_dbName);
            var myUserStore = new UserStore<User>(context);
            var userManager = BuildUserManager<User>(myUserStore);      
            MockAuth(httpContext);
            var signInManager = SetupSignInManager(userManager, httpContext);
            return  new AuthController(signInManager, jwtServiceMock.Object, configuration);
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
