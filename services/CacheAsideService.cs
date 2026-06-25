using jwtmanualauthentication.Models.Enities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace jwtmanualauthentication.services
{
    public static class CacheAsideService // we defined class as static because the method uses extensions method
    {

        private static readonly DistributedCacheEntryOptions Default = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        };

        private static readonly SemaphoreSlim semaphore = new(1, 1); //initial thread count = 1 and maximum threads can it have = 1

        public static async Task<T> GetOrCreateAsync<T>(
            this IDistributedCache cache,
            string key,
            Func<CancellationToken, Task<T>> factory,
            CancellationToken ct,
            DistributedCacheEntryOptions options = null
            ) {
            
            var cachedValue = await cache.GetStringAsync(key, ct);

            T value;
            //we deserialize it using the string 
            if (!string.IsNullOrEmpty(cachedValue))
            {
                value = JsonSerializer.Deserialize<T>(cachedValue);

                // if user presents after deserializing then return in the results
                if (value is not null)
                {
                    return value;
                }
                return default;
            }
            // if user data does not present in the cache then take from the db anb convert it into string and write data to the cache

            bool hasLock = await semaphore.WaitAsync(500);

            if (!hasLock) {
                return default;
            }

            try
            {
                cachedValue = await cache.GetStringAsync(key, ct);
                if (!string.IsNullOrEmpty(cachedValue)) {
                    return JsonSerializer.Deserialize<T>(cachedValue); ;
                }

                value = await factory(ct);
                if (value != null)
                {
                    await cache.SetStringAsync(key, JsonSerializer.Serialize(value), options ?? Default, ct);
                    return value;
                }
                return default;
            }
            catch (Exception ex)
            {
                return default;
            }
            finally { 
                semaphore.Release();
            }
        }
    }
}
