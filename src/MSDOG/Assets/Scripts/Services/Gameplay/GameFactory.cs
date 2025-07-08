using Constants;
using Core;

namespace Services.Gameplay
{
    public class GameFactory
    {
        private readonly AssetProviderService _assetProviderService;
        private readonly InputService _inputService;
        private readonly ArenaService _arenaService;
        private readonly UpdateService _updateService;

        public GameFactory(AssetProviderService assetProviderService, UpdateService updateService, InputService inputService,
            ArenaService arenaService)
        {
            _assetProviderService = assetProviderService;
            _inputService = inputService;
            _arenaService = arenaService;
            _updateService = updateService;
        }

        public Player CreatePlayer()
        {
            var player = _assetProviderService.Instantiate<Player>(AssetPaths.PlayerPrefab);
            player.Init(_inputService, _updateService, _arenaService);
            return player;
        }
    }
}