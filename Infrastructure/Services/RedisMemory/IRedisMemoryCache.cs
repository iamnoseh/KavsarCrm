namespace Infrastructure.Services.Memory;

public interface IRedisMemoryCache
{
    Task SetDataAsync<T>(string key, T Data, int expiration);
    Task<T?> GetDataAsync<T>(string key);
    Task RemoveDataAsync(string key);
}