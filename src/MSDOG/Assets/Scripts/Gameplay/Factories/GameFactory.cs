using Constants;
using Core.Controllers;
using Core.Models.Data;
using Core.Services;
using Gameplay.Controllers;
using Gameplay.Enemies;
using Gameplay.Services;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.Factories
{
    public class GameFactory
    {
        private readonly AssetProviderService _assetProviderService;
        private readonly InputService _inputService;
        private readonly ArenaService _arenaService;
        private readonly AbilityFactory _abilityFactory;
        private readonly ProjectileFactory _projectileFactory;
        private readonly VfxFactory _vfxFactory;
        private readonly DataService _dataService;
        private readonly RuntimeContainers _runtimeContainers;
        private readonly DebugService _debugService;
        private readonly ProgressService _progressService;
        private readonly UpdateService _updateService;

        private Player _player; // TODO: remove?

        public Player Player => _player;

        public GameFactory(AssetProviderService assetProviderService, UpdateService updateService, InputService inputService,
            ArenaService arenaService, AbilityFactory abilityFactory, ProjectileFactory projectileFactory,
            VfxFactory vfxFactory, DataService dataService, RuntimeContainers runtimeContainers, DebugService debugService,
            ProgressService progressService)
        {
            _assetProviderService = assetProviderService;
            _inputService = inputService;
            _arenaService = arenaService;
            _abilityFactory = abilityFactory;
            _projectileFactory = projectileFactory;
            _vfxFactory = vfxFactory;
            _dataService = dataService;
            _runtimeContainers = runtimeContainers;
            _debugService = debugService;
            _progressService = progressService;
            _updateService = updateService;
        }

        public Player CreatePlayer()
        {
            var player = _assetProviderService.Instantiate<Player>(AssetPaths.PlayerPrefab);
            player.Init(_inputService, _updateService, _arenaService, _abilityFactory, _dataService, _progressService);
            _player = player;
            return player;
        }

        public Enemy CreateEnemy(Vector3 position, EnemyData data)
        {
            // TODO: init via container
            var enemy = Object.Instantiate(data.Prefab, position, Quaternion.identity, _runtimeContainers.EnemyContainer);
            enemy.Init(_updateService, this, _projectileFactory, _player, data, _vfxFactory, _debugService);
            return enemy;
        }

        public EnemyDeathkit CreateEnemyDeathkit(EnemyDeathkit deathkitPrefab, Vector3 position, Quaternion rotation)
        {
            var enemyDeathkit = Object.Instantiate(deathkitPrefab, position, rotation, _runtimeContainers.EnemyContainer);
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