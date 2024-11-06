namespace DigitalAssetManagement.UseCases.Common
{
    public interface IMessageConsumer
    {
        void EstablishConnection();
        void Consume<TMessage>(Action<TMessage> messageProcessor);
        void Dispose();
    }
}
