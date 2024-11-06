namespace DigitalAssetManagement.UseCases
{
    public interface ICache
    {
        void Remove(object key);
        void Set<TValue>(object key, TValue value);
        bool TryGetValue<TValue>(object key, out TValue value);
    }
}
