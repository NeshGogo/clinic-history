namespace DoctorService.Entities
{
    public class Doctor : BaseEntity
    {
        public string? UserId { get; set; }
        public string FullName { get; set; }
        public string Identification { get; set; }
        public string SpecialityId { get; set; }
        public Speciality Speciality { get; set; }
        public User? User { get; set; }
    }
}
