using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Linq;
using System.Text;


namespace inMemoryApi.Utility
{
    public static class DistributedCachingExtension
    {
        public static T GetCacheStrongly<T>(this IDistributedCache cache, string key) where T : class
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }
            var buffer = cache.Get(key);
            if (buffer == null || !buffer.Any())
            {
                return default;
            }
            var json = Encoding.UTF8.GetString(buffer);
            T value = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return value;
        }
    }
}
