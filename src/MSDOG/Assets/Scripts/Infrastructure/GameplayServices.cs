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

        public static void Initialize(AssetProviderService assetProviderService, SoundService soundService,
            UpdateService updateService, DataService dataService)
        {
            var inputService = new InputService();
            var arenaService = new ArenaService();
            var projectileFactory = new ProjectileFactory(assetProviderService, updateService);
            var abilityFactory = new AbilityFactory(projectileFactory);
            var gameFactory = new GameFactory(assetProviderService, updateService, inputService, arenaService, abilityFactory);

            var cameraService = assetProviderService.Instantiate<CameraService>(AssetPaths.CameraServicePath);
            cameraService.Init(updateService);

            var enemyService = new EnemyService(updateService, dataService, gameFactory, arenaService);

            _gameFactory = gameFactory;
            _cameraService = cameraService;
            _enemyService = enemyService;
        }

        public static void Start()
        {
            var player = _gameFactory.CreatePlayer();
            _cameraService.SetFollowTarget(player.transform);

            var hudController = Object.FindFirstObjectByType<HudController>();
            hudController.Init(player);

            _enemyService.ActivateLevel(0, player.transform); // TODO: add level selection
        }

        public static void Cleanup()
        {
            _enemyService.Cleanup();
        }
    }
}