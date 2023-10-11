using AccountService.DTOs;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AccountService.AsyncDataService
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MessageBusClient> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration config, ILogger<MessageBusClient> logger)
        {
            _config = config;
            _logger = logger;
            var rabbitmqConfig = _config.GetSection("RabbitMQ");
            var factory = new ConnectionFactory()
            {
                HostName = rabbitmqConfig.GetValue<string>("Host"),
                Port = rabbitmqConfig.GetValue<int>("Port"),
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(rabbitmqConfig.GetValue<string>("Exchange"), type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                _logger.LogInformation($"--> Connected to message bus");
            }
            catch (Exception ex)
            {
                _logger.LogError($"--> Could not connect to the message bus: {ex.Message}");
            }
        }


        public void PublishNewUser(UserPublishDTO userPublishDTO)
        {
            var message = JsonSerializer.Serialize(userPublishDTO);
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

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation($"--> RabbitMQ connection shutdown");
        }
    }
}
