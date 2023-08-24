namespace AccountService.DTOs
{
    public class UserDTO
    {
        public string FullName { get; set; }
        public string Identification { get; set; }
        public string UserType { get; set; }
        public DateTime RecordCreated { get; set; }
        public string RecordCreatedBy { get; set; }
        public bool Active { get; protected set; }
    }
}
