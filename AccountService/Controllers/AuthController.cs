using AccountService.DTOs;
using AccountService.Entities;
using AccountService.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;

        public AuthController(SignInManager<User> signInManager, IJwtService jwtService)
        {
            _signInManager = signInManager;
            _jwtService = jwtService;
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
    }
}
