using Constants;
using Services;

namespace Infrastructure.StateMachine
{
    public class MainMenuState : IResolvableState, IState
    {
        private LoadingCurtainService _loadingCurtainService;
        private SceneLoadService _sceneLoadService;

        public void Resolve()
        {
            _loadingCurtainService = GlobalServices.LoadingCurtainService;
            _sceneLoadService = GlobalServices.SceneLoadService;
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