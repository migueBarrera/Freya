﻿namespace FreyaMobile.Core.Services;

public class SessionService : ISessionService
{
    private readonly ConcurrentDictionary<string, object> cache = new ConcurrentDictionary<string, object>();

    public SessionService()
    {
    }

    public T Get<T>(string key) where T : class
    {
        if (cache.TryGetValue(key, out object? data) 
            && data is T tData)
        {
            return tData;
        }
        else
        {
            return default!;
        }
    }

    public void Save<T>(string key, T data) where T : class
    {
        cache.AddOrUpdate(key, data, (_, __) => data);
    }

    public void Remove(string key)
    {
        cache.TryRemove(key, out object? o);
    }

    public void Clear()
    {
        cache.Clear();
    }
}