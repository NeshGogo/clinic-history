using Microsoft.AspNetCore.Identity;

namespace AccountService.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string Identification { get; set; }
        public string UserType { get; set; }
    }
}
