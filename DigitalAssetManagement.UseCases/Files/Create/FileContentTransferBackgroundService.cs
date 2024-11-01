using DigitalAssetManagement.UseCases.Common;
using Microsoft.Extensions.Hosting;

namespace DigitalAssetManagement.UseCases.Files.Create
{
    public class FileContentTransferBackgroundService(FileCreation fileCreation, MessageConsumer messageConsumer) : BackgroundService
    {
        private readonly FileCreation _fileCreation = fileCreation;
        private readonly MessageConsumer _messageConsumer = messageConsumer;

        public override Task StartAsync(CancellationToken cancellationToken)
        {
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
            _messageConsumer.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
