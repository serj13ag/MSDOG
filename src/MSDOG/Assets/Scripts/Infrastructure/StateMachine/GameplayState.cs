using Constants;
using Core.Controllers;
using Core.Services;
using Gameplay.Factories;
using UnityEngine;
using VContainer;

namespace Infrastructure.StateMachine
{
    public class GameplayState : IPayloadedState<int>
    {
        private int _levelIndex;

        private LoadingCurtainController _loadingCurtainController;
        private SceneLoadService _sceneLoadService;
        private WindowController _windowController;

        [Inject]
        public void Construct(LoadingCurtainController loadingCurtainController, SceneLoadService sceneLoadService,
            WindowController windowController)
        {
            _windowController = windowController;
            _loadingCurtainController = loadingCurtainController;
            _sceneLoadService = sceneLoadService;
        }

        public void Enter(int levelIndex)
        {
            _levelIndex = levelIndex;

            _loadingCurtainController.FadeOnInstantly();
            _sceneLoadService.LoadScene(Settings.SceneNames.LevelSceneName, OnSceneLoaded, true);
        }

        public void Exit()
        {
            _windowController.RemoveGameplayWindowFactory();
            _windowController.CloseAllWindows();
        }

        private void OnSceneLoaded()
        {
            var gameplayScope = Object.FindFirstObjectByType<GameplayLifetimeScope>();
            gameplayScope.BuildContainer();

            var gameplayWindowFactory = gameplayScope.Container.Resolve<GameplayWindowFactory>();
            _windowController.RegisterGameplayWindowFactory(gameplayWindowFactory);

            var gameplayInitializer = gameplayScope.Container.Resolve<GameplayInitializer>();
            gameplayInitializer.Start(_levelIndex);

            _loadingCurtainController.FadeOffWithDelay();
        }
    }
}