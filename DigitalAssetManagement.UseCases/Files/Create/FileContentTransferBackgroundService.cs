using DigitalAssetManagement.UseCases.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DigitalAssetManagement.UseCases.Files.Create
{
    public class FileContentTransferBackgroundService(IServiceProvider serviceProvider, IMessageConsumer messageConsumer) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly IMessageConsumer _messageConsumer = messageConsumer;
        private IServiceScope _serviceScope;
        private FileCreation _fileCreation;

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _serviceScope = _serviceProvider.CreateScope();
            _fileCreation = _serviceScope.ServiceProvider.GetRequiredService<FileCreation>();
            
            _messageConsumer.EstablishConnection();
            _messageConsumer.Consume<FileChunkUploadRequest>((request) => _fileCreation.ProcessFileChunkUploadAsync(request));
            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _serviceScope.Dispose();
            _messageConsumer.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
