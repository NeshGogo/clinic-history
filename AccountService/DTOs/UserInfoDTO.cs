using System.ComponentModel.DataAnnotations;

namespace AccountService.DTOs
{
    public class UserInfoDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
