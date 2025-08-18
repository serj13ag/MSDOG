using System.Linq;
using System.Threading.Tasks;
using Constants;
using Core.Controllers;
using Core.Services;
using Gameplay.Factories;
using UnityEngine.SceneManagement;
using VContainer;

namespace Infrastructure.StateMachine
{
    public class GameplayState : IPayloadedState<int>
    {
        private int _levelIndex;

        private ILoadingCurtainController _loadingCurtainController;
        private ISceneLoadService _sceneLoadService;
        private IWindowController _windowController;

        [Inject]
        public void Construct(ILoadingCurtainController loadingCurtainController, ISceneLoadService sceneLoadService,
            IWindowController windowController)
        {
            _windowController = windowController;
            _loadingCurtainController = loadingCurtainController;
            _sceneLoadService = sceneLoadService;
        }

        public void Enter(int levelIndex)
        {
            _levelIndex = levelIndex;

            _loadingCurtainController.FadeOnInstantly();

            _ = LoadGameplay();
        }

        public void Exit()
        {
            _windowController.RemoveGameplayWindowFactory();
            _windowController.CloseAllWindows();

            _ = UnloadAdditiveTvScene();
        }

        private async Task LoadGameplay()
        {
            var loadLevel = _sceneLoadService.LoadSceneAsync(Settings.SceneNames.LevelSceneName);
            var loadLevelTv = _sceneLoadService.LoadSceneAsync(Settings.SceneNames.LevelTvHudSceneName, LoadSceneMode.Additive);

            await Task.WhenAll(loadLevel, loadLevelTv);

            OnScenesLoaded(loadLevel.Result, loadLevelTv.Result);
        }

        private async Task UnloadAdditiveTvScene()
        {
            await _sceneLoadService.UnloadSceneAsync(Settings.SceneNames.LevelTvHudSceneName);
        }

        private void OnScenesLoaded(Scene levelScene, Scene levelTvScene)
        {
            var gameplayScope = levelScene.GetRootGameObjects()
                .Select(gameObject => gameObject.GetComponent<GameplayLifetimeScope>())
                .First();
            gameplayScope.BuildContainer();

            var gameplayTvHudScope = levelTvScene.GetRootGameObjects()
                .Select(go => go.GetComponent<GameplayTvHudLifetimeScope>())
                .First();
            gameplayTvHudScope.BuildContainer();

            var gameplayWindowFactory = gameplayScope.Container.Resolve<IGameplayWindowFactory>();
            _windowController.RegisterGameplayWindowFactory(gameplayWindowFactory);

            var gameplayInitializer = gameplayScope.Container.Resolve<GameplayInitializer>();
            gameplayInitializer.Start(_levelIndex); // TODO: add await for factories warmup ?

            var tvInitializer = gameplayTvHudScope.Container.Resolve<GameplayTvHudInitializer>();
            tvInitializer.Start();

            _loadingCurtainController.FadeOffWithDelay();
        }
    }
}