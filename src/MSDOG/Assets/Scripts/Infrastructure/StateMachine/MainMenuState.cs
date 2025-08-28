using Common;
using Core.Controllers;
using Core.Services;
using VContainer;

namespace Infrastructure.StateMachine
{
    public class MainMenuState : IState
    {
        private ILoadingCurtainController _loadingCurtainController;
        private ISceneLoadService _sceneLoadService;

        [Inject]
        public void Construct(ILoadingCurtainController loadingCurtainController, ISceneLoadService sceneLoadService)
        {
            _loadingCurtainController = loadingCurtainController;
            _sceneLoadService = sceneLoadService;
        }

        public void Enter()
        {
            _loadingCurtainController.FadeOnInstantly();
            _sceneLoadService.LoadScene(Constants.SceneNames.MenuSceneName, OnSceneLoaded);
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