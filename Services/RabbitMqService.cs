using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace GarageQueueDownload.Services
{
    public class RabbitMqService : IDisposable
    {
        private readonly string _hostName;
        private readonly string _queueName;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService(string hostName, string queueName, int port, string userName, string password)
        {
            _hostName = hostName;
            _queueName = queueName;

            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                Port = port,
                UserName = userName,
                Password = password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            Console.WriteLine($"Connected to RabbitMQ at {_hostName}");
        }

        public void SendMessage(object message)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            _channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _channel.BasicPublish(
                exchange: "",
                routingKey: _queueName,
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"Message sent to queue {_queueName}");
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
