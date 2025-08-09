using System;

namespace Core.Services
{
    public interface ISceneLoadService
    {
        void LoadScene(string name, Action onLoaded = null, bool forceReload = false);
    }
}