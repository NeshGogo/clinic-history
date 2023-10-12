using AccountService.DTOs;
using AccountService.Entities;
using AccountService.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _config;

        public AuthController(SignInManager<User> signInManager, IJwtService jwtService, IConfiguration config)
        {
            _signInManager = signInManager;
            _jwtService = jwtService;
            _config = config;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserTokenDTO>> Login([FromBody] UserInfoDTO userInfoDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfoDTO.Email, userInfoDTO.Password, false, false);
            if (!result.Succeeded) return BadRequest("Invalid login attempt");
            return await _jwtService.BuildToken(userInfoDTO);
        }

        [HttpPost("RenewToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserTokenDTO>> Renovate()
        {
            var userinfo = new UserInfoDTO() { Email = HttpContext.User.Identity.Name };
            return await _jwtService.BuildToken(userinfo);
        }

        [HttpPost("Authorized")]
        public async Task<ActionResult<bool>> Authorized([FromBody] string token)
        {
            try
            {
                var tokeValidatorParams = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                           Encoding.UTF8.GetBytes(_config["jwt:key"])),
                    ClockSkew = TimeSpan.Zero
                };
                token = token.Replace("Bearer ", string.Empty);
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, tokeValidatorParams, out var validatedtoke);
                return validatedtoke is not null;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
