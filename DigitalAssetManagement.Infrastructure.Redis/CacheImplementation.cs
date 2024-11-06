using DigitalAssetManagement.UseCases;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace DigitalAssetManagement.Infrastructure.Redis
{
    public class CacheImplementation(IDistributedCache cache): ICache
    {
        private readonly IDistributedCache _distributedCache = cache;

        public void Remove(object key)
        {
            _distributedCache.Remove((string) key);
        }

        public void Set<TValue>(object key, TValue value)
        {
            _distributedCache.SetString((string)key, value.ToString());
        }

        public bool TryGetValue<TValue>(object key, out TValue value)
        {
            var a = _distributedCache.GetString((string)key);
            value = JsonConvert.DeserializeObject<TValue>(a);
            if (a == null)
                return false;
            return true;
        }
    }
}
