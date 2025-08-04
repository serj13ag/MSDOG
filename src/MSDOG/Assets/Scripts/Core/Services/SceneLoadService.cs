using System;
using System.Collections;
using Core.Controllers;
using UnityEngine.SceneManagement;

namespace Core.Services
{
    public class SceneLoadService
    {
        private readonly CoroutineController _coroutineController;

        public SceneLoadService(CoroutineController coroutineController)
        {
            _coroutineController = coroutineController;
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