using Constants;
using Core.Controllers;
using Core.Services;
using Gameplay.Services;
using UnityEngine;
using VContainer;

namespace Infrastructure.StateMachine
{
    public class GameplayState : IPayloadedState<int>
    {
        private int _levelIndex;

        private LoadingCurtainService _loadingCurtainService;
        private SceneLoadService _sceneLoadService;
        private WindowService _windowService;

        [Inject]
        public void Construct(LoadingCurtainService loadingCurtainService, SceneLoadService sceneLoadService,
            WindowService windowService)
        {
            _windowService = windowService;
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
            _windowService.RemoveGameplayWindowFactory();
            _windowService.CloseAllWindows();
        }

        private void OnSceneLoaded()
        {
            var gameplayScope = Object.FindFirstObjectByType<GameplayServicesScope>();
            gameplayScope.BuildContainer();

            var gameplayWindowFactory = gameplayScope.Container.Resolve<GameplayWindowFactory>();
            _windowService.RegisterGameplayWindowFactory(gameplayWindowFactory);

            var gameplayInitializer = gameplayScope.Container.Resolve<GameplayInitializer>();
            gameplayInitializer.Start(_levelIndex);

            _loadingCurtainService.FadeOffWithDelay();
        }
    }
}