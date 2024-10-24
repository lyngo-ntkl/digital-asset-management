using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace DigitalAssetManagement.Infrastructure.Common
{
    public class RabbitMQBackgroundService(SystemFileHelper fileHelper, ILogger<RabbitMQBackgroundService> logger, IMemoryCache cache, IServiceProvider serviceProvider) : BackgroundService
    {
        private const string exchangeName = "hi, this is exchange";
        private const string queueName = "request-queue";
        private const string routingKey = "routing key is shorter than 255 bytes";
        private readonly SystemFileHelper _fileHelper = fileHelper;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<RabbitMQBackgroundService> _logger = logger;
        private IConnection? _connection;
        private IModel? _channel;
        private readonly IMemoryCache _fileChunkTracker = cache;

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            EstablishConnection();
            ConsumeMessage();
            return base.StartAsync(cancellationToken);
        }

        public void EstablishConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(queueName, durable: false, exclusive: false);
            _channel.QueueBind(queueName, exchangeName, routingKey);

            _channel.BasicQos(0, 1, false);
        }

        public void ConsumeMessage()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, args) => await ProcessMessage(sender, args);
            _channel.BasicConsume(queueName, true, consumer);
        }

        public async Task ProcessMessage(object? sender, BasicDeliverEventArgs args)
        {
            var fileChunk = JsonConvert.DeserializeObject<FileChunkUploadRequest>(
                Encoding.UTF8.GetString(args.Body.ToArray())
            );

            if (fileChunk?.TotalChunk > 1)
            {
                var path = _fileHelper.AddFile(fileChunk.ChunkData, AbsolutePathCreationHelper.CreateAbsolutePath($"{fileChunk.FileId}.{fileChunk.ChunkNumber}.part"));
                FileChunkUploadTracking? fileChunkTracking;
                if (_fileChunkTracker.TryGetValue(fileChunk.FileId, out fileChunkTracking))
                {
                    if (fileChunkTracking?.ArrivedChunks.Count == fileChunk.TotalChunk - 1)
                    {
                        fileChunkTracking.ArrivedChunks.Add(fileChunk.ChunkNumber, path);
                        await MergeFileChunk(fileChunk.FileId, fileChunkTracking.ArrivedChunks);
                        _fileChunkTracker.Remove(fileChunk.FileId);
                    }
                    else
                    {
                        var arrivedChunks = fileChunkTracking?.ArrivedChunks;
                        arrivedChunks.Add(fileChunk.ChunkNumber, path);
                        _fileChunkTracker.Set(fileChunk.FileId, fileChunkTracking);
                    }
                }
                else
                {
                    fileChunkTracking = new FileChunkUploadTracking
                    {
                        TotalChunk = fileChunk.TotalChunk,
                        ArrivedChunks = new Dictionary<int, string>
                            {
                                {fileChunk.ChunkNumber, path}
                            }
                    };
                    _fileChunkTracker.Set(fileChunk.FileId, fileChunkTracking);
                }
            }
            else
            {
                await AddFile(fileChunk.ChunkData, fileChunk.FileId);
            }
        }

        public async Task MergeFileChunk(int fileId, Dictionary<int, string> arrivedChunk)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var metadataService = scope.ServiceProvider.GetService<MetadataService>();
                var file = await metadataService!.GetFileMetadataById(fileId);
                var fileChunkPaths = arrivedChunk.OrderBy(pair => pair.Key).ToDictionary().Values.ToList();
                _fileHelper.MergeFile(file.AbsolutePath, fileChunkPaths);
            }
        }

        public async Task AddFile(byte[] data, int fileId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var metadataService = scope.ServiceProvider.GetService<MetadataService>();
                var file = await metadataService!.GetFileMetadataById(fileId);
                _fileHelper.AddFile(data, file.AbsolutePath);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {}

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}
