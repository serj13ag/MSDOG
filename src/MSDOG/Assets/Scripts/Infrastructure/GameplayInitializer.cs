using Services;
using Services.Gameplay;
using UI.HUD;
using UI.HUD.Actions;
using UnityEngine;

namespace Infrastructure
{
    public class GameplayInitializer
    {
        private readonly DebugService _debugService;
        private readonly EnemyService _enemyService;
        private readonly GameFactory _gameFactory;
        private readonly CameraService _cameraService;
        private readonly DataService _dataService;
        private readonly AssetProviderService _assetProviderService;
        private readonly WindowService _windowService;
        private readonly SoundService _soundService;
        private readonly GameStateService _gameStateService;

        public GameplayInitializer(DebugService debugService, EnemyService enemyService, GameFactory gameFactory,
            CameraService cameraService, DataService dataService, AssetProviderService assetProviderService,
            WindowService windowService, SoundService soundService, GameStateService gameStateService)
        {
            _debugService = debugService;
            _enemyService = enemyService;
            _gameFactory = gameFactory;
            _cameraService = cameraService;
            _dataService = dataService;
            _assetProviderService = assetProviderService;
            _windowService = windowService;
            _soundService = soundService;
            _gameStateService = gameStateService;
        }

        public void Start(int levelIndex)
        {
            _windowService.CreateRootCanvas();

            var player = _gameFactory.CreatePlayer();

            _gameStateService.RegisterPlayer(player, levelIndex);
            _cameraService.SetFollowTarget(player.transform);
            _enemyService.ActivateLevel(levelIndex, player.transform);

            var hudController = Object.FindFirstObjectByType<HudController>();
            hudController.Init(player, _dataService, _assetProviderService, _soundService);
            hudController.AddStartAbility();

            var hudActions = Object.FindFirstObjectByType<HudActions>();
            hudActions.Init(player);

            _debugService.Setup(hudController);
        }
    }
}