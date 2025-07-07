using Constants;
using Services;

namespace Infrastructure.StateMachine
{
    public class MainMenuState : IResolvableState
    {
        private LoadingCurtainService _loadingCurtainService;
        private SceneLoadService _sceneLoadService;
        // private IUiFactory _uiFactory;

        public void Resolve()
        {
            _loadingCurtainService = GlobalServices.LoadingCurtainService;
            _sceneLoadService = GlobalServices.SceneLoadService;
            // _uiFactory = globalServices.UiFactory;
        }

        public void Enter()
        {
            _loadingCurtainService.FadeOnInstantly();
            _sceneLoadService.LoadScene(Settings.SceneNames.MenuSceneName, OnSceneLoaded);
        }

        public void Exit()
        {
            // _uiFactory.Cleanup();
        }

        private void OnSceneLoaded()
        {
            // _uiFactory.CreateUiRootCanvas();
            // _uiFactory.CreateMainMenu();

            _loadingCurtainService.FadeOffWithDelay();
        }
    }
}