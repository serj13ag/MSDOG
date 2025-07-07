using Constants;
using Services;
using UnityEngine;

namespace Infrastructure
{
    public static class GlobalServices
    {
        public static LoadingCurtainService LoadingCurtainService { get; private set; }
        public static SceneLoadService SceneLoadService { get; private set; }

        public static void Initialize()
        {
            var coroutineService = new GameObject("CoroutineService")
                .AddComponent<CoroutineService>();

            var assetProviderService = new AssetProviderService();
            var sceneLoadService = new SceneLoadService(coroutineService);
            var loadingCurtainService =
                assetProviderService.Instantiate<LoadingCurtainService>(AssetPaths.LoadingCurtainServicePath);

            LoadingCurtainService = loadingCurtainService;
            SceneLoadService = sceneLoadService;
        }
    }
}