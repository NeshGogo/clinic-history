namespace DoctorService.Dtos
{
    public class BaseDto
    {
        public string Id { get; set; }
        public DateTime RecordCreated { get; protected set; }
        public bool Active { get; protected set; } = true;
    }
}
