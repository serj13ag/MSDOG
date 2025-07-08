using Constants;
using Services;
using Services.Gameplay;

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
            var gameFactory = new GameFactory(assetProviderService, updateService, inputService, arenaService);

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

            _enemyService.ActivateLevel(0); // TODO: add level selection
        }

        public static void Cleanup()
        {
            _enemyService.Cleanup();
        }
    }
}