namespace HistoryService.Dtos
{
    public class ClinicRecordDto : DtoBase
    {
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string Diagnosis { get; set; }
        public PatientDto Patient { get; set; }
        public DoctorDto Doctor { get; set; }
    }
}
