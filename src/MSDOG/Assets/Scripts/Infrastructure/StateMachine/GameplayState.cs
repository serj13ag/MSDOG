using Constants;
using Services;
using UnityEngine;
using VContainer;

namespace Infrastructure.StateMachine
{
    public class GameplayState : IResolvableState, IPayloadedState<int>
    {
        private int _levelIndex;

        private LoadingCurtainService _loadingCurtainService;
        private SceneLoadService _sceneLoadService;

        public void Resolve()
        {
            _loadingCurtainService = GlobalServices.LoadingCurtainService;
            _sceneLoadService = GlobalServices.SceneLoadService;
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

            GameplayServices.Initialize(gameplayScope.Container);

            var gameplayInitializer = gameplayScope.Container.Resolve<GameplayInitializer>();
            gameplayInitializer.Start(_levelIndex);

            _loadingCurtainService.FadeOffWithDelay();
        }
    }
}