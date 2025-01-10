using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GarageQueueUpload.Services
{
    public class RabbitMqService
    {
        private readonly string _hostName;
        private readonly string _queueName;

        // Ändra konstruktorn så att den tar emot RabbitMqConfig istället för IConfiguration
        public RabbitMqService(RabbitMqConfig config)
        {
            _hostName = config.HostName;
            _queueName = config.QueueName;
        }

        public void SendMessage(object message)
        {
            var factory = new ConnectionFactory { HostName = _hostName };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublish(
                exchange: "",
                routingKey: _queueName,
                basicProperties: null,
                body: body
            );
        }
    }
}
