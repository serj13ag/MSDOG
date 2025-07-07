using Constants;
using Services;

namespace Infrastructure.StateMachine
{
    public class GameplayState : IResolvableState
    {
        private AssetProviderService _assetProviderService;
        private LoadingCurtainService _loadingCurtainService;
        private SceneLoadService _sceneLoadService;
        private SoundService _soundService;
        private UpdateService _updateService;

        public void Resolve()
        {
            _assetProviderService = GlobalServices.AssetProviderService;
            _loadingCurtainService = GlobalServices.LoadingCurtainService;
            _sceneLoadService = GlobalServices.SceneLoadService;
            _soundService = GlobalServices.SoundService;
            _updateService = GlobalServices.UpdateService;
        }

        public void Enter()
        {
            _loadingCurtainService.FadeOnInstantly();
            _sceneLoadService.LoadScene(Settings.SceneNames.LevelSceneName, OnSceneLoaded);
        }

        public void Exit()
        {
            GameplayServices.Cleanup();
        }

        private void OnSceneLoaded()
        {
            GameplayServices.Initialize(_assetProviderService, _soundService, _updateService);
            GameplayServices.Start();

            _loadingCurtainService.FadeOffWithDelay();
        }
    }
}