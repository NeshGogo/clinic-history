using HistoryService.Entities;

namespace HistoryService.SyncDataServices.Grpc
{
    public interface IDoctorDataClient
    {
        IEnumerable<Doctor> ReturnsAllDoctors();
    }
}
