using DigitalAssetManagement.UseCases.Common;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalAssetManagement.Infrastructure.RabbitMQ
{
    public static class RabbitMQExtensions
    {
        public static void AddRabbitMQ(this IServiceCollection services)
        {
            services.AddSingleton<IMessageConsumer, MessageConsumerImplementation>();
        }
    }
}
