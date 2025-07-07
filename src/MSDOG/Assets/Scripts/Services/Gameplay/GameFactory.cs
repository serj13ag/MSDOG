using Constants;
using Core;

namespace Services.Gameplay
{
    public class GameFactory
    {
        private readonly AssetProviderService _assetProviderService;
        private readonly InputService _inputService;
        private readonly UpdateService _updateService;

        public GameFactory(AssetProviderService assetProviderService, InputService inputService, UpdateService updateService)
        {
            _assetProviderService = assetProviderService;
            _inputService = inputService;
            _updateService = updateService;
        }

        public Player CreatePlayer()
        {
            var player = _assetProviderService.Instantiate<Player>(AssetPaths.PlayerPrefab);
            player.Init(_inputService, _updateService);
            return player;
        }
    }
}