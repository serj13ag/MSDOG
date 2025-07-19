using Constants;
using Services;
using Services.Gameplay;
using UI.HUD;
using UnityEngine;

namespace Infrastructure
{
    public static class GameplayServices
    {
        private static EnemyService _enemyService;
        private static GameFactory _gameFactory;
        private static CameraService _cameraService;
        private static DataService _dataService;
        private static AssetProviderService _assetProviderService;
        private static WindowService _windowService;

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

            _assetProviderService = assetProviderService;
            _dataService = dataService;
            _windowService = windowService;

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
            hudController.Init(player, _dataService, _assetProviderService);
            hudController.AddStartAbility();

            _enemyService.ActivateLevel(GameStateService.CurrentLevelIndex, player.transform);
        }

        public static void Cleanup()
        {
            _enemyService.Cleanup();
            GameStateService.Cleanup();
        }
    }
}