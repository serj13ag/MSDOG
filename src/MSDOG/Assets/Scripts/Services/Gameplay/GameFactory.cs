using System;
using Constants;
using Core;
using Core.Enemies;
using Data;
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

        public Enemy CreateEnemy(Vector3 position, EnemyData data)
        {
            var prefabPath = data.Type switch
            {
                EnemyType.Wanderer => AssetPaths.WandererEnemyPrefab,
                EnemyType.Melee => AssetPaths.MeleeEnemyPrefab,
                _ => throw new ArgumentOutOfRangeException(),
            };

            var enemy = _assetProviderService.Instantiate<Enemy>(prefabPath, position);
            enemy.Init(_updateService, this, _player, data);
            return enemy;
        }

        public ExperiencePiece CreateExperiencePiece(Vector3 position)
        {
            var experiencePiece = _assetProviderService.Instantiate<ExperiencePiece>(AssetPaths.ExperiencePiecePrefab, position);
            experiencePiece.Init(_updateService);
            return experiencePiece;
        }
    }
}