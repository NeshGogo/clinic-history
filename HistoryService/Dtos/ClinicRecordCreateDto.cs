using System.ComponentModel.DataAnnotations;

namespace HistoryService.Dtos
{
    public class ClinicRecordCreateDto
    {
        [Required]
        [MaxLength(36)]
        [MinLength(36)]
        public string DoctorId { get; set; }
        [Required]
        [MaxLength(36)]
        [MinLength(36)]
        public string PatientId { get; set; }
        [Required]
        [MaxLength(500)]
        public string Diagnosis { get; set; }
    }
}
