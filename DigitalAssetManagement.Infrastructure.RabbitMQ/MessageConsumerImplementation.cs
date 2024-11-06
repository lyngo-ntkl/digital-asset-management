using DigitalAssetManagement.UseCases.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace DigitalAssetManagement.Infrastructure.RabbitMQ
{
    public class MessageConsumerImplementation(IConfiguration configuration) : IMessageConsumer
    {
        private readonly IConfiguration _configuration = configuration;
        private IConnection? _connection;
        private IModel? _channel;
        private const string ExchangeName = "exchange-name";
        private const string Type = ExchangeType.Direct;
        private const string QueueName = "request-queue";
        private const string RoutingKey = "routing key is shorter than 255 bytes";
        public void Consume<TMessage>(Action<TMessage> messageProcessor)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, args) =>
            {
                var request = JsonConvert.DeserializeObject<TMessage>(
                    Encoding.UTF8.GetString(args.Body.ToArray())
                );
                messageProcessor(request!);
            };
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        public void EstablishConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["rabbitmq:host"]
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: ExchangeName, type: Type, durable: false, autoDelete: false);
            _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false);
            _channel.QueueBind(QueueName, ExchangeName, RoutingKey);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 0, global: false);
        }
    }
}
