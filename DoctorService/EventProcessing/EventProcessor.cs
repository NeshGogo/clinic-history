using AutoMapper;
using System.Text.Json;
using DoctorService.Dtos;
using DoctorService.Enums;
using DoctorService.Data;
using DoctorService.Entities;

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
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.NewUser:
                    AddUser(message);
                    break;
                default:
                    break;
            }
        }

        private void AddUser(string userPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IBaseRepository<User>>();
                var platformPublishDTO = JsonSerializer.Deserialize<UserPublishMessageDto>(userPublishedMessage);
                try
                {
                    var user = _mapper.Map<User>(platformPublishDTO);
                    if (!repo.Exists(p => p.ExternalId == user.ExternalId))
                    {
                        repo.Add(user);
                        repo.SaveChanges();
                    }
                    else
                        _logger.LogWarning("--> user already exisits...");

                }
                catch (Exception ex)
                {
                    _logger.LogError($"--> Could not add user to DB {ex.Message}");
                }

            }
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
