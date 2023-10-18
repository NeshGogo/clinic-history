using AccountService.DTOs;

namespace AccountService.Services
{
    public interface IJwtService
    {
        Task<UserTokenDTO> BuildToken(UserInfoDTO userInfo);
        bool ValidateToke(string token);
    }
}
