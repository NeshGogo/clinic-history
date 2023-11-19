using AutoMapper;
using HistoryService.Data;
using HistoryService.Dtos;
using HistoryService.Entities;
using HistoryService.Enums;
using System.Text.Json;

namespace HistoryService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private ILogger<EventProcessor> _logger;
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EventProcessor(ILogger<EventProcessor> logger, IMapper mapper, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _mapper = mapper;
            _serviceScopeFactory = serviceScopeFactory;
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

        private void AddUser(string userPublishedMessage)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IBaseRepo<User>>();
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
    }
}
