using DoctorService.Dtos;

namespace DoctorService.AsyncDataService
{
    public interface IMessageBusClient
    {
        void PublishNewDoctor(DoctorPublishDto doctorPublish);
    }
}
