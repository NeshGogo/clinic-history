using AutoMapper;
using System.Text.Json;
using DoctorService.Dtos;
using DoctorService.Enums;

namespace DoctorService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly ILogger<EventProcessor> _logger;
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(ILogger<EventProcessor> logger, IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public void ProcessEvent(string message)
        {
            throw new NotImplementedException();
        }



        private string DetermineEvent(string notificationMessage)
        {
            _logger.LogInformation("--> Determining Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case EventType.NewUser:
                    _logger.LogInformation("--> New user event detected");
                    return EventType.NewUser;
                default:
                    _logger.LogInformation("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }
    }
}
