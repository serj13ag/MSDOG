using Constants;
using Services;
using Services.Gameplay;

namespace Infrastructure
{
    public static class GameplayServices
    {
        private static GameFactory _gameFactory;
        private static CameraService _cameraService;

        public static void Initialize(AssetProviderService assetProviderService, SoundService soundService,
            UpdateService updateService)
        {
            var inputService = new InputService();
            var arenaService = new ArenaService();
            var gameFactory = new GameFactory(assetProviderService, updateService, inputService, arenaService);
            var cameraService = assetProviderService.Instantiate<CameraService>(AssetPaths.CameraServicePath);
            cameraService.Init(updateService);

            _gameFactory = gameFactory;
            _cameraService = cameraService;
        }

        public static void Start()
        {
            var player = _gameFactory.CreatePlayer();
            _cameraService.SetFollowTarget(player.transform);
        }

        public static void Cleanup()
        {
        }
    }
}