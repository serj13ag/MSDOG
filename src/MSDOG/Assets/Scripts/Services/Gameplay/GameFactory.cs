using Constants;
using Core;
using Core.Enemies;
using UnityEngine;

namespace Services.Gameplay
{
    public class GameFactory
    {
        private readonly AssetProviderService _assetProviderService;
        private readonly InputService _inputService;
        private readonly ArenaService _arenaService;
        private readonly AbilityFactory _abilityFactory;
        private readonly UpdateService _updateService;

        private Player _player; // TODO: remove?

        public GameFactory(AssetProviderService assetProviderService, UpdateService updateService, InputService inputService,
            ArenaService arenaService, AbilityFactory abilityFactory)
        {
            _assetProviderService = assetProviderService;
            _inputService = inputService;
            _arenaService = arenaService;
            _abilityFactory = abilityFactory;
            _updateService = updateService;
        }

        public Player CreatePlayer()
        {
            var player = _assetProviderService.Instantiate<Player>(AssetPaths.PlayerPrefab);
            player.Init(_inputService, _updateService, _arenaService, _abilityFactory);
            _player = player;
            return player;
        }

        public Enemy CreateEnemy(Vector3 position)
        {
            var enemy = _assetProviderService.Instantiate<Enemy>(AssetPaths.EnemyPrefab, position);
            enemy.Init(_updateService, _player);
            return enemy;
        }
    }
}