using Constants;
using Core.Controllers;
using Core.Services;
using VContainer;

namespace Infrastructure.StateMachine
{
    public class MainMenuState : IState
    {
        private LoadingCurtainController _loadingCurtainController;
        private SceneLoadService _sceneLoadService;

        [Inject]
        public void Construct(LoadingCurtainController loadingCurtainController, SceneLoadService sceneLoadService)
        {
            _loadingCurtainController = loadingCurtainController;
            _sceneLoadService = sceneLoadService;
        }

        public void Enter()
        {
            _loadingCurtainController.FadeOnInstantly();
            _sceneLoadService.LoadScene(Settings.SceneNames.MenuSceneName, OnSceneLoaded);
        }

        public void Exit()
        {
        }

        private void OnSceneLoaded()
        {
            _loadingCurtainController.FadeOffWithDelay();
        }
    }
}