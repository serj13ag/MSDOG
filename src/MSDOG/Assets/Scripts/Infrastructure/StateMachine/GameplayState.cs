using Constants;
using Services;
using UnityEngine;
using VContainer;

namespace Infrastructure.StateMachine
{
    public class GameplayState : IPayloadedState<int>
    {
        private int _levelIndex;

        private LoadingCurtainService _loadingCurtainService;
        private SceneLoadService _sceneLoadService;

        [Inject]
        public void Construct(LoadingCurtainService  loadingCurtainService, SceneLoadService sceneLoadService)
        {
            _loadingCurtainService = loadingCurtainService;
            _sceneLoadService = sceneLoadService;
        }

        public void Enter(int levelIndex)
        {
            _levelIndex = levelIndex;

            _loadingCurtainService.FadeOnInstantly();
            _sceneLoadService.LoadScene(Settings.SceneNames.LevelSceneName, OnSceneLoaded, true);
        }

        public void Exit()
        {
        }

        private void OnSceneLoaded()
        {
            var gameplayScope = Object.FindFirstObjectByType<GameplayServicesScope>();
            gameplayScope.BuildContainer();

            var gameplayInitializer = gameplayScope.Container.Resolve<GameplayInitializer>();
            gameplayInitializer.Start(_levelIndex);

            _loadingCurtainService.FadeOffWithDelay();
        }
    }
}