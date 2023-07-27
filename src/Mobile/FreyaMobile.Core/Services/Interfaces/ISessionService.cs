namespace FreyaMobile.Core.Services.Interfaces;

public interface ISessionService
{
    void Save<T>(string key, T data) where T : class;

    T Get<T>(string key) where T : class;

    void Remove(string key);

    void Clear();
}
