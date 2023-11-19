namespace HistoryService.Entities
{
    public class ClinicRecord
    {
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string Diagnosis { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}