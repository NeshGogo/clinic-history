namespace HistoryService.Entities
{
    public class User : BaseEntity
    {
        public string ExternalId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
