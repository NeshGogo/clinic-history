using Microsoft.AspNetCore.Identity;

namespace AccountService.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string Identification { get; set; }
        public string UserType { get; set; }
        public DateTime RecordCreated { get;  set; } = DateTime.Now;
        public string RecordCreatedBy { get; set; }
        public DateTime RecordUpdated { get; set; } = DateTime.Now;
        public string RecordUpdatedBy { get; set; }
        public bool Active { get; protected set; } = true;
    }
}
