using Constants;
using Core.Services;
using VContainer;

namespace Infrastructure.StateMachine
{
    public class MainMenuState : IState
    {
        private LoadingCurtainService _loadingCurtainService;
        private SceneLoadService _sceneLoadService;

        [Inject]
        public void Construct(LoadingCurtainService loadingCurtainService, SceneLoadService sceneLoadService)
        {
            _loadingCurtainService = loadingCurtainService;
            _sceneLoadService = sceneLoadService;
        }

        public void Enter()
        {
            _loadingCurtainService.FadeOnInstantly();
            _sceneLoadService.LoadScene(Settings.SceneNames.MenuSceneName, OnSceneLoaded);
        }

        public void Exit()
        {
        }

        private void OnSceneLoaded()
        {
            _loadingCurtainService.FadeOffWithDelay();
        }
    }
}