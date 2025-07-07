namespace Infrastructure.StateMachine
{
    public class MainMenuState : IResolvableState
    {
        // private ILoadingCurtainService _loadingCurtainService;
        // private ISceneLoadService _sceneLoadService;
        // private IUiFactory _uiFactory;

        public void Resolve()
        {
            // _loadingCurtainService = globalServices.LoadingCurtainService;
            // _sceneLoadService = globalServices.SceneLoadService;
            // _uiFactory = globalServices.UiFactory;
        }

        public void Enter()
        {
            // _loadingCurtainService.FadeOnInstantly();
            // _sceneLoadService.LoadScene("Level", OnSceneLoaded);
        }

        public void Exit()
        {
            // _uiFactory.Cleanup();
        }

        private void OnSceneLoaded()
        {
            // _uiFactory.CreateUiRootCanvas();
            // _uiFactory.CreateMainMenu();
            //
            // _loadingCurtainService.FadeOffWithDelay();
        }
    }
}