using HistoryService.EventProcessing;

namespace HistoryService.AsyncDataService
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly ILogger<MessageBusSubscriber> _logger;
        private readonly IConfiguration _config;
        private readonly IEventProcessor _eventProcessor;

        public MessageBusSubscriber(ILogger<MessageBusSubscriber> logger, IConfiguration config, IEventProcessor eventProcessor)
        {
            _logger = logger;
            _config = config;
            _eventProcessor = eventProcessor;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
