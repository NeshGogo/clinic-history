using DoctorService.Entities;

namespace DoctorService.SyncDataServices.Grpc
{
    public interface IUserDataClient
    {
        IEnumerable<User> ReturnsAllUsers();
    }
}
