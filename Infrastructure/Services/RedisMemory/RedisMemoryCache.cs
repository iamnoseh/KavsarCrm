using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Memory;

public class RedisMemoryCache(IDistributedCache cache,ILogger<RedisMemoryCache> logger) : IRedisMemoryCache
{
    public async Task SetDataAsync<T>(string key, T Data, int expiration)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expiration),
        };
        var jsonSerializerOption = new JsonSerializerOptions { WriteIndented = true };
        var jsonData = JsonSerializer.Serialize(Data, jsonSerializerOption);
        
        await cache.SetStringAsync(key, jsonData, options);
        
        
        await Console.Out.WriteLineAsync(new string('-',100));
        logger.LogInformation("Memory cache : Add new data to cache by key : {key}, expiration time before : {exprirationTime}", key, expiration);
        await Console.Out.WriteLineAsync(new string('-',100));

    }

    public async Task<T?> GetDataAsync<T>(string key)
    {
        var data = await cache.GetStringAsync(key);
        await Console.Out.WriteLineAsync(new string('-',100));
        logger.LogInformation("Memory cache : Data retrieved from cache  key : {key}", key);
        await Console.Out.WriteLineAsync(new string('*', 120));
        return data != null
            ? JsonSerializer.Deserialize<T>(data)
            : default;
    }

    public async Task RemoveDataAsync(string key)
    {
        await cache.RemoveAsync(key);
        await Console.Out.WriteLineAsync(new string('-',100));
        logger.LogInformation("Memory cache : Deleted data in cache by key : {key}", key);
        await Console.Out.WriteLineAsync(new string('*', 100));
    }
}