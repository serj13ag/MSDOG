using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Core.Services
{
    public class SceneLoadService
    {
        private readonly CoroutineService _coroutineService;

        public SceneLoadService(CoroutineService coroutineService)
        {
            _coroutineService = coroutineService;
        }

        public void LoadScene(string name, Action onLoaded = null, bool forceReload = false)
        {
            if (!forceReload && SceneManager.GetActiveScene().name == name)
            {
                onLoaded?.Invoke();
                return;
            }

            _coroutineService.StartCoroutine(LoadSceneRoutine(name, onLoaded));
        }

        private IEnumerator LoadSceneRoutine(string name, Action onLoaded)
        {
            var loadSceneAsync = SceneManager.LoadSceneAsync(name);

            while (!loadSceneAsync.isDone)
            {
                yield return null;
            }

            onLoaded?.Invoke();
        }
    }
}