using System.ComponentModel.DataAnnotations;

namespace DoctorService.Dtos
{
    public class SpecialityCreateDto
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(800)]
        public string? Description { get; set; }
    }
}
