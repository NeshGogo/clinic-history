using System.ComponentModel.DataAnnotations;

namespace HistoryService.Dtos
{
    public class PatientCreateDto
    {
        [MaxLength(128)]
        [Required]
        public string FullName { get; set; }
        [MaxLength(11)]
        [MinLength(11)]
        [Required]
        public string Identification { get; set; }
        [MaxLength(20)]
        [Required]
        public string Sex { get; set; }
        [MaxLength(36)]
        [MinLength(36)]
        public string? UserId { get; set; }
    }
}
