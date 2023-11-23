namespace HistoryService.Entities
{
    public class Doctor : BaseEntity
    {
        public string ExternalId { get; set; }
        public string FullName { get; set; }
        public string Speciality { get; set; }
    }
}
