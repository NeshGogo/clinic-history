using DoctorService.Entities;
using System.ComponentModel.DataAnnotations;

namespace DoctorService.Dtos
{
    public class DoctorCreateDto
    {
        [Required]
        [MaxLength(228)]
        public string FullName { get; set; }
        [MaxLength(36)]
        [MinLength(36)]
        [Required]
        public string SpecialityId { get; set; }
    }
}
