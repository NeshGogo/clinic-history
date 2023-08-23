using AccountService.DTOs;
using AccountService.Entities;
using AccountService.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public AccountsController(UserManager<User> userManager, IMapper mapper, IJwtService jwtService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<UserTokenDTO>> Create([FromBody] UserCreateDTO createDTO)
        {
            var user = _mapper.Map<User>(createDTO);
            user.UserName = createDTO.Email;
            var result = await _userManager.CreateAsync(user, createDTO.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            var userInfo = _mapper.Map<UserInfoDTO>(createDTO);
            var token = await _jwtService.BuildToken(userInfo);
            return token;
        }
    }
}
