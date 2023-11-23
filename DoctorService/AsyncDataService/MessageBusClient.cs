using DoctorService.Dtos;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System.Threading.Channels;

namespace DoctorService.AsyncDataService
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly ILogger<MessageBusClient> _logger;
        private readonly IConfiguration _config;
        private IConnection _connection;
        private IModel _channel;

        public MessageBusClient(ILogger<MessageBusClient> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            InitializeRabbitMQ();
        }
        public void PublishNewDoctor(DoctorPublishDto doctorPublish)
        {
            var message = JsonSerializer.Serialize(doctorPublish);
            if (_connection != null && _connection.IsOpen)
            {
                _logger.LogInformation("--> RabbitMQ connection Open, Sending message...");
                SendMessage(message);
            }
            else
            {
                _logger.LogWarning("--> RabbitMQ connection closed, not sening");
            }
        }
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
              exchange: _config.GetSection("RabbitMQ").GetValue<string>("Exchange"),
              routingKey: "",
              basicProperties: null,
              body: body);
            _logger.LogInformation($"--> We have sent {message}");
        }

        private void InitializeRabbitMQ()
        {
            try
            {
                var rabbitmqConfig = _config.GetSection("RabbitMQ");
                var factory = new ConnectionFactory
                {
                    HostName = rabbitmqConfig.GetValue<string>("Host"),
                    Port = rabbitmqConfig.GetValue<int>("Port"),
                };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(
                    exchange: rabbitmqConfig.GetValue<string>("Exchange"),
                    ExchangeType.Fanout);
                _logger.LogInformation("--> Listenting on the Message Bus...");
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Clould not connect to MessageBus because of: {ex.Message}");
            }
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("--> Connection Shutdown...");
        }
    }
}
