using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.Services
{
    public interface ISceneLoadService
    {
        void LoadScene(string name, Action onLoaded = null, bool forceReload = false);
        Task<Scene> LoadSceneAsync(string name, LoadSceneMode mode = LoadSceneMode.Single);
        Task UnloadSceneAsync(string name);
    }
}