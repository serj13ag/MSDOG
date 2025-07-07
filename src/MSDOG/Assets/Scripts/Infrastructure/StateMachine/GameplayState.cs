using Constants;
using Services;

namespace Infrastructure.StateMachine
{
    public class GameplayState : IResolvableState
    {
        // private IAssetProviderService _assetProviderService;
        private LoadingCurtainService _loadingCurtainService;

        private SceneLoadService _sceneLoadService;
        // private IUpdateService _updateService;
        // private ISoundService _soundService;
        //
        // private IGameplayServices _gameplayServices;

        public void Resolve()
        {
            // _assetProviderService = globalServices.AssetProviderService;
            _loadingCurtainService = GlobalServices.LoadingCurtainService;
            _sceneLoadService = GlobalServices.SceneLoadService;
            // _updateService = globalServices.UpdateService;
            // _soundService = globalServices.SoundService;
        }

        public void Enter()
        {
            _loadingCurtainService.FadeOnInstantly();
            _sceneLoadService.LoadScene(Settings.SceneNames.LevelSceneName, OnSceneLoaded);
        }

        public void Exit()
        {
            // _gameplayServices.Cleanup();
            // _gameplayServices = null;
        }

        private void OnSceneLoaded()
        {
            // _gameplayServices = new GameplayServices(_updateService, _assetProviderService, _soundService);
            //
            // _gameplayServices.Start();

            _loadingCurtainService.FadeOffWithDelay();
        }
    }
}