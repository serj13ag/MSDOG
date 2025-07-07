using Constants;
using Services;
using UnityEngine;

namespace Infrastructure
{
    public static class GlobalServices
    {
        public static AssetProviderService AssetProviderService { get; private set; }
        public static LoadingCurtainService LoadingCurtainService { get; private set; }
        public static SceneLoadService SceneLoadService { get; private set; }
        public static SoundService SoundService { get; private set; }
        public static UpdateService UpdateService { get; private set; }

        public static void Initialize()
        {
            var coroutineService = new GameObject("CoroutineService")
                .AddComponent<CoroutineService>();
            var updateService = new GameObject("UpdateService")
                .AddComponent<UpdateService>();

            var assetProviderService = new AssetProviderService();
            var sceneLoadService = new SceneLoadService(coroutineService);
            var loadingCurtainService =
                assetProviderService.Instantiate<LoadingCurtainService>(AssetPaths.LoadingCurtainServicePath);

            var soundService = assetProviderService.Instantiate<SoundService>(AssetPaths.SoundServicePath);
            soundService.Init(assetProviderService);

            UpdateService = updateService;
            AssetProviderService = assetProviderService;
            LoadingCurtainService = loadingCurtainService;
            SceneLoadService = sceneLoadService;
            SoundService = soundService;
        }
    }
}