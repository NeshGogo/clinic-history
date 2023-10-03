using AccountService.DTOs;

namespace AccountService.AsyncDataService
{
    public interface IMessageBusClient
    {
        void PublishNewUser(UserPublishDTO userPublishDTO);
    }
}
