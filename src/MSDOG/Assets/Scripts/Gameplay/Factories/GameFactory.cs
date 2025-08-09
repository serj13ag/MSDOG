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
        private readonly IAssetProviderService _assetProviderService;
        private readonly InputService _inputService;
        private readonly ArenaService _arenaService;
        private readonly AbilityFactory _abilityFactory;
        private readonly ProjectileFactory _projectileFactory;
        private readonly VfxFactory _vfxFactory;
        private readonly IDataService _dataService;
        private readonly ObjectContainerProvider _objectContainerProvider;
        private readonly IDebugController _debugController;
        private readonly IProgressService _progressService;
        private readonly IUpdateController _updateController;

        private Player _player; // TODO: remove?

        public GameFactory(IAssetProviderService assetProviderService,
            IUpdateController updateController,
            InputService inputService,
            ArenaService arenaService,
            AbilityFactory abilityFactory,
            ProjectileFactory projectileFactory,
            VfxFactory vfxFactory,
            IDataService dataService,
            ObjectContainerProvider objectContainerProvider,
            IDebugController debugController,
            IProgressService progressService)
        {
            _assetProviderService = assetProviderService;
            _inputService = inputService;
            _arenaService = arenaService;
            _abilityFactory = abilityFactory;
            _projectileFactory = projectileFactory;
            _vfxFactory = vfxFactory;
            _dataService = dataService;
            _objectContainerProvider = objectContainerProvider;
            _debugController = debugController;
            _progressService = progressService;
            _updateController = updateController;
        }

        public Player CreatePlayer()
        {
            var player = _assetProviderService.Instantiate<Player>(AssetPaths.PlayerPrefab);
            player.Init(_inputService, _updateController, _arenaService, _abilityFactory, _dataService, _progressService);
            _player = player;
            return player;
        }

        public Enemy CreateEnemy(Vector3 position, EnemyData data)
        {
            // TODO: init via container
            var enemy = Object.Instantiate(data.Prefab, position, Quaternion.identity, _objectContainerProvider.EnemyContainer);
            enemy.Init(_updateController, this, _projectileFactory, _player, data, _vfxFactory, _debugController, _dataService);
            return enemy;
        }

        public ExperiencePiece CreateExperiencePiece(Vector3 position, int experience)
        {
            var experiencePiece = _assetProviderService.Instantiate<ExperiencePiece>(AssetPaths.ExperiencePiecePrefab, position);
            experiencePiece.Init(experience, _updateController);
            return experiencePiece;
        }
    }
}