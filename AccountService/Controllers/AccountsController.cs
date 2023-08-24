using AccountService.Data.Repositories;
using AccountService.DTOs;
using AccountService.Entities;
using AccountService.Enums;
using AccountService.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserRepository _repository;

        public AccountsController(
            UserManager<User> userManager,
            IMapper mapper,
            IJwtService jwtService,
            IUserRepository repository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtService = jwtService;
            _repository = repository;
        }

        [HttpGet("Users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<IList<UserDTO>> GetUsers()
        {
            var results = _repository.GetAll();
            return _mapper.Map<List<UserDTO>>(results);
        }

        [HttpGet("UserTypes")]
        public ActionResult<IList<string>> GetUserTypes()
        {
            return UserType.GetList().ToList();
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserTokenDTO>> CreateUser([FromBody] UserCreateDTO createDTO)
        {
            var isUserTypeValid = UserType.IsUserTypeValid(createDTO.UserType);
            if (!isUserTypeValid) return BadRequest("UserType is not valid");  
            var user = _mapper.Map<User>(createDTO);
            user.UserName = createDTO.Email;
            user.RecordCreatedBy = createDTO.Email;
            user.RecordUpdatedBy = createDTO.Email;
            var result = await _userManager.CreateAsync(user, createDTO.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            var userInfo = _mapper.Map<UserInfoDTO>(createDTO);
            var token = await _jwtService.BuildToken(userInfo);
            return token;
        }

        [HttpPut("update/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserDTO>> UpdateUser(string id, [FromBody] UserCreateDTO updateDTO)
        {
            var user = await _repository.FindById(id);
            if (user == null) return NotFound($"Could not find the user with ID {id}");
            _mapper.Map(updateDTO, user);
            _repository.Update(user, "System");
            var result = await _repository.SaveChanges();
            if (!result) return BadRequest($"--> Something get wrong trying to update the user with ID {id}");
            return _mapper.Map<UserDTO>(user);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var exists = await _repository.Exists(id);
            if (!exists) return NotFound($"Could not find the user with ID {id}");
            _repository.Delete(id);
            var result = await _repository.SaveChanges();
            if (!result) return BadRequest($"--> Something get wrong trying to delete the user with ID {id}");
            return NoContent();
        }
    }
}
