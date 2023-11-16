namespace HistoryService.Entities
{
    public class Patient : BaseEntity
    {
        public string FullName { get; set; }
        public string Identification { get; set; }
        public string Sex { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
