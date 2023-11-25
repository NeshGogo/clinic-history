using HistoryService.Entities;

namespace HistoryService.SyncDataServices.Grpc
{
    public interface IUserDataClient
    {
        IEnumerable<User> ReturnsAllUsers();
    }
}
