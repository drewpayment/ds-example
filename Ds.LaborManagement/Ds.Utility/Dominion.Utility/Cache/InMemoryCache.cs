using System;
using System.Diagnostics;
using System.Runtime.Caching;

namespace Dominion.Utility.Cache
{
    public class InMemoryCache : ICacheService
    {
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            if (MemoryCache.Default.Get(cacheKey) is T item)
            {
                return item;
            }

            item = getItemCallback();
            MemoryCache.Default.AddOrGetExisting(cacheKey, item, GetPolicy());

            return item;
        }

        public void Clear(string cacheKey)
        {
            MemoryCache.Default.Remove(cacheKey);
        }

        private CacheItemPolicy GetPolicy()
        {
            return new CacheItemPolicy
            {
                SlidingExpiration = TimeSpan.FromMinutes(2),
                RemovedCallback = args => { Debug.WriteLine(args.CacheItem.Key + "-->" + args.RemovedReason); }
            };
        }
    }

    internal interface ICacheService
    {
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class;
        void Clear(string cacheKey);
    }
}