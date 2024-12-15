using Microsoft.Extensions.Caching.Memory;
using System;

namespace RockPaperScissorsGame.Infrastructure.Caching
{
    /// <summary>
    /// Provides methods for managing in-memory caching.
    /// </summary>
    public class MemoryCacheManager
    {
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheManager"/> class.
        /// </summary>
        /// <param name="cache">The memory cache instance.</param>
        public MemoryCacheManager(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        /// <summary>
        /// Retrieves an item from the cache or creates and caches a new item if it does not exist.
        /// </summary>
        /// <typeparam name="T">The type of the cached item.</typeparam>
        /// <param name="key">The unique key for the cached item. Must not be null.</param>
        /// <param name="createItem">A function to create the item if it does not exist in the cache.</param>
        /// <returns>The cached item.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the key or createItem is null.</exception>
        public T GetOrCreate<T>(string key, Func<ICacheEntry, T> createItem)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Cache key cannot be null or empty.", nameof(key));
            }

            if (createItem == null)
            {
                throw new ArgumentNullException(nameof(createItem), "Create item function cannot be null.");
            }

            return _cache.GetOrCreate(key, createItem);
        }

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="key">The unique key for the cached item. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when the key is null or empty.</exception>
        public void Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Cache key cannot be null or empty.", nameof(key));
            }

            _cache.Remove(key);
        }
    }
}
