namespace Sisa.Abstractions;

public interface ICaching
{

    Task<TData> GetOrSetAsync<TData>(
        string key,
        Func<Task<TData>> dataRetrievalFunc,
        TimeSpan expiration,
        CancellationToken cancellationToken = default);

    Task<TData?> GetAsync<TData>(string key, CancellationToken cancellationToken = default);

    Task<bool> SetAsync<TData>(string key, TData data, TimeSpan expiration, CancellationToken cancellationToken = default);

    TData GetOrSet<TData>(
        string key,
        Func<TData> dataRetrievalFunc,
        TimeSpan expiration);

    TData? Get<TData>(string key);

    bool Set<TData>(string key, TData data, TimeSpan expiration);
}
