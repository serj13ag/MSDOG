namespace Core.Services
{
    public interface ISaveLoadService
    {
        T Load<T>(string key) where T : new();
        void Save<T>(T saveData, string key);
    }
}