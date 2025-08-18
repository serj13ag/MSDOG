using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Controllers;
using UnityEngine.SceneManagement;

namespace Core.Services
{
    public class SceneLoadService : ISceneLoadService
    {
        private readonly ICoroutineController _coroutineController;

        public SceneLoadService(ICoroutineController coroutineController)
        {
            _coroutineController = coroutineController;
        }

        public async Task<Scene> LoadSceneAsync(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            var loadOp = SceneManager.LoadSceneAsync(name, mode);
            while (!loadOp.isDone)
            {
                await Task.Yield();
            }

            return SceneManager.GetSceneByName(name);
        }

        public async Task UnloadSceneAsync(string name)
        {
            var unloadOp = SceneManager.UnloadSceneAsync(name);
            while (!unloadOp.isDone)
            {
                await Task.Yield();
            }
        }

        public void LoadScene(string name, Action onLoaded = null, bool forceReload = false)
        {
            if (!forceReload && SceneManager.GetActiveScene().name == name)
            {
                onLoaded?.Invoke();
                return;
            }

            _coroutineController.StartCoroutine(LoadSceneRoutine(name), onLoaded);
        }

        private IEnumerator LoadSceneRoutine(string name)
        {
            var loadSceneAsync = SceneManager.LoadSceneAsync(name);

            while (!loadSceneAsync.isDone)
            {
                yield return null;
            }
        }
    }
}