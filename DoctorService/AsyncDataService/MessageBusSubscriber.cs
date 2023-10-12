using DoctorService.EventProcessing;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace DoctorService.AsyncDataService
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MessageBusSubscriber> _logger;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(
            IConfiguration config, 
            ILogger<MessageBusSubscriber> logger, 
            IEventProcessor eventProcessor)
        {
            _config = config;
            _logger = logger;
            _eventProcessor = eventProcessor;
            InitializeRabbitMQ();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ModuleHandle, ea) => {
                _logger.LogInformation("--> Event Received!");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                _eventProcessor.ProcessEvent(notificationMessage);
            };
            _channel?.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
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
                _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(
                    queue: _queueName,
                    exchange: rabbitmqConfig.GetValue<string>("Exchange"),
                    routingKey: "");
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
