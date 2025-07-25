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
        private readonly ProjectileFactory _projectileFactory;
        private readonly ParticleFactory _particleFactory;
        private readonly UpdateService _updateService;

        private Player _player; // TODO: remove?

        public Player Player => _player;

        public GameFactory(AssetProviderService assetProviderService, UpdateService updateService, InputService inputService,
            ArenaService arenaService, AbilityFactory abilityFactory, ProjectileFactory projectileFactory,
            ParticleFactory particleFactory)
        {
            _assetProviderService = assetProviderService;
            _inputService = inputService;
            _arenaService = arenaService;
            _abilityFactory = abilityFactory;
            _projectileFactory = projectileFactory;
            _particleFactory = particleFactory;
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
                EnemyType.Range => AssetPaths.RangeEnemyPrefab,
                _ => throw new ArgumentOutOfRangeException(),
            };

            var enemy = _assetProviderService.Instantiate<Enemy>(prefabPath, position);
            enemy.Init(_updateService, this, _projectileFactory, _player, data, _particleFactory);
            return enemy;
        }

        public EnemyDeathkit CreateEnemyDeathkit(EnemyType enemyType, Vector3 position, Quaternion rotation)
        {
            var prefabPath = enemyType switch
            {
                EnemyType.Wanderer => AssetPaths.WandererEnemyDeathkitPrefab,
                EnemyType.Melee => AssetPaths.MeleeEnemyDeathkitPrefab,
                EnemyType.Range => AssetPaths.RangeEnemyDeathkitPrefab,
                _ => throw new ArgumentOutOfRangeException(),
            };

            var enemyDeathkit = _assetProviderService.Instantiate<EnemyDeathkit>(prefabPath, position, rotation);
            enemyDeathkit.Init();
            return enemyDeathkit;
        }

        public ExperiencePiece CreateExperiencePiece(Vector3 position, int experience)
        {
            var experiencePiece = _assetProviderService.Instantiate<ExperiencePiece>(AssetPaths.ExperiencePiecePrefab, position);
            experiencePiece.Init(experience, _updateService);
            return experiencePiece;
        }
    }
}