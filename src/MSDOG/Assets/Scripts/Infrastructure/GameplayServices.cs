using Constants;
using Services;
using Services.Gameplay;
using UI.HUD;
using UI.HUD.Actions;
using UnityEngine;

namespace Infrastructure
{
    public static class GameplayServices
    {
        private static DebugService _debugService;
        private static EnemyService _enemyService;
        private static GameFactory _gameFactory;
        private static CameraService _cameraService;
        private static DataService _dataService;
        private static AssetProviderService _assetProviderService;
        private static WindowService _windowService;
        private static SoundService _soundService;

        public static InputService InputService { get; private set; }
        public static GameStateService GameStateService { get; private set; }

        public static void Initialize(int levelIndex, AssetProviderService assetProviderService, SoundService soundService,
            UpdateService updateService, DataService dataService, WindowService windowService, ProgressService progressService)
        {
            var inputService = new InputService();
            var arenaService = new ArenaService();
            var projectileFactory = new ProjectileFactory(assetProviderService, updateService);
            var abilityFactory = new AbilityFactory(projectileFactory);
            var gameFactory = new GameFactory(assetProviderService, updateService, inputService, arenaService,
                abilityFactory, projectileFactory);

            var cameraService = assetProviderService.Instantiate<CameraService>(AssetPaths.CameraServicePath);
            cameraService.Init(updateService);

            var enemyService = new EnemyService(updateService, dataService, gameFactory, arenaService);
            var gameStateService = new GameStateService(levelIndex, enemyService, windowService, progressService);

            var debugService = new GameObject("DebugService").AddComponent<DebugService>();
            debugService.Init(updateService);

            _soundService = soundService;
            _assetProviderService = assetProviderService;
            _dataService = dataService;
            _windowService = windowService;
            _debugService = debugService;

            _gameFactory = gameFactory;
            _cameraService = cameraService;
            _enemyService = enemyService;

            InputService = inputService;
            GameStateService = gameStateService;
        }

        public static void Start()
        {
            _windowService.CreateRootCanvas();

            var player = _gameFactory.CreatePlayer();
            GameStateService.RegisterPlayer(player);
            _cameraService.SetFollowTarget(player.transform);

            var hudController = Object.FindFirstObjectByType<HudController>();
            hudController.Init(player, _dataService, _assetProviderService, _soundService);
            hudController.AddStartAbility();

            var hudActions = Object.FindFirstObjectByType<HudActions>();
            hudActions.Init(player);
            
            _debugService.Setup(hudController);
                
            _enemyService.ActivateLevel(GameStateService.CurrentLevelIndex, player.transform);
        }

        public static void Cleanup()
        {
            _enemyService.Cleanup();
            GameStateService.Cleanup();
        }
    }
}