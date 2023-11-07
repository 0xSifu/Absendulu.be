using AbsenDulu.BE.Interfaces.IServices.Cache;
using Alachisoft.NCache.Client;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AbsenDulu.BE.Services.Cache;
public class CacheService
    {
        private readonly ICache _cache;
 
        public CacheService(ICache cache)
        {
            _cache = cache;
        }
 
        // public T Get<T>(string key)
        // {
        //     var value = _cache.Get<string>(key);
 
        //     if (value != null)
        //     {
        //         return JsonSerializer.Deserialize<T>(value);
        //     }
 
        //     return default;
        // }
 
        // public T Set<T>(string key, T value)
        // {
   
        //     _cache.Insert(key, JsonSerializer.Serialize(value));
 
        //     return value;
        // }
    }