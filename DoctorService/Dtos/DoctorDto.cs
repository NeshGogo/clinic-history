using DoctorService.Entities;

namespace DoctorService.Dtos
{
    public class DoctorDto : BaseDto
    {
        public string FullName { get; set; }
        public string SpecialityId { get; set; }
        public string Identification { get; set; }
        public SpecialityDto Speciality { get; set; }
    }
}
