using Constants;
using Services;

namespace Infrastructure.StateMachine
{
    public class GameplayState : IResolvableState, IPayloadedState<int>
    {
        private int _levelIndex;

        private AssetProviderService _assetProviderService;
        private LoadingCurtainService _loadingCurtainService;
        private SceneLoadService _sceneLoadService;
        private SoundService _soundService;
        private UpdateService _updateService;
        private DataService _dataService;
        private WindowService _windowService;
        private ProgressService _progressService;

        public void Resolve()
        {
            _assetProviderService = GlobalServices.AssetProviderService;
            _loadingCurtainService = GlobalServices.LoadingCurtainService;
            _sceneLoadService = GlobalServices.SceneLoadService;
            _soundService = GlobalServices.SoundService;
            _updateService = GlobalServices.UpdateService;
            _dataService = GlobalServices.DataService;
            _windowService = GlobalServices.WindowService;
            _progressService = GlobalServices.ProgressService;
        }

        public void Enter(int levelIndex)
        {
            _levelIndex = levelIndex;

            _loadingCurtainService.FadeOnInstantly();
            _sceneLoadService.LoadScene(Settings.SceneNames.LevelSceneName, OnSceneLoaded, true);
        }

        public void Exit()
        {
            GameplayServices.Cleanup();
        }

        private void OnSceneLoaded()
        {
            GameplayServices.Initialize(_levelIndex, _assetProviderService, _soundService, _updateService, _dataService,
                _windowService, _progressService);
            GameplayServices.Start();

            _loadingCurtainService.FadeOffWithDelay();
        }
    }
}