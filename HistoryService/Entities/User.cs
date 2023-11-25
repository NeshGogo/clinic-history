namespace HistoryService.Entities
{
    public class User : BaseEntity, IExternalId
    {
        public string ExternalId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
