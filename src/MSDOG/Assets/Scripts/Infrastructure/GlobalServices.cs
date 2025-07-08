using Constants;
using Infrastructure.StateMachine;
using Services;
using UnityEngine;

namespace Infrastructure
{
    public static class GlobalServices
    {
        public static GameStateMachine GameStateMachine { get; private set; }

        public static AssetProviderService AssetProviderService { get; private set; }
        public static DataService DataService { get; private set; }
        public static LoadingCurtainService LoadingCurtainService { get; private set; }
        public static SceneLoadService SceneLoadService { get; private set; }
        public static SoundService SoundService { get; private set; }
        public static UpdateService UpdateService { get; private set; }

        public static void Initialize(GameStateMachine gameStateMachine)
        {
            GameStateMachine = gameStateMachine;

            var coroutineService = new GameObject("CoroutineService")
                .AddComponent<CoroutineService>();
            var updateService = new GameObject("UpdateService")
                .AddComponent<UpdateService>();

            var assetProviderService = new AssetProviderService();
            var dataService = new DataService();
            var sceneLoadService = new SceneLoadService(coroutineService);
            var loadingCurtainService =
                assetProviderService.Instantiate<LoadingCurtainService>(AssetPaths.LoadingCurtainServicePath);

            var soundService = assetProviderService.Instantiate<SoundService>(AssetPaths.SoundServicePath);
            soundService.Init(assetProviderService);

            UpdateService = updateService;
            AssetProviderService = assetProviderService;
            DataService = dataService;
            LoadingCurtainService = loadingCurtainService;
            SceneLoadService = sceneLoadService;
            SoundService = soundService;
        }
    }
}