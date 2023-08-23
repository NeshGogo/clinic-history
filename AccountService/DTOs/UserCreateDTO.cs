using System.ComponentModel.DataAnnotations;

namespace AccountService.DTOs
{
    public class UserCreateDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(20)]
        public string UserType { get; set; }
        [Required]
        [MaxLength(11)]
        public string Identification { get; set; }
    }
}
