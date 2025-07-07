using Constants;
using Core;

namespace Services.Gameplay
{
    public class GameFactory
    {
        private readonly AssetProviderService _assetProviderService;

        public GameFactory(AssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
        }

        public Player CreatePlayer()
        {
            var player = _assetProviderService.Instantiate<Player>(AssetPaths.PlayerPrefab);
            return player;
        }
    }
}