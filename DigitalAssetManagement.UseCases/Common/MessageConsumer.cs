namespace DigitalAssetManagement.UseCases.Common
{
    public interface MessageConsumer
    {
        void EstablishConnection();
        void Consume<TMessage>(Action<TMessage> messageProcessor);
        void Dispose();
    }
}
