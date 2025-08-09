using Constants;
using Core.Controllers;
using Core.Models.Data;
using Core.Services;
using Gameplay.Controllers;
using Gameplay.Enemies;
using Gameplay.Providers;
using Gameplay.Services;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.Factories
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProviderService _assetProviderService;
        private readonly IInputService _inputService;
        private readonly IArenaService _arenaService;
        private readonly IAbilityFactory _abilityFactory;
        private readonly IProjectileFactory _projectileFactory;
        private readonly IVfxFactory _vfxFactory;
        private readonly IDataService _dataService;
        private readonly IObjectContainerProvider _objectContainerProvider;
        private readonly IDebugController _debugController;
        private readonly IProgressService _progressService;
        private readonly IExperiencePieceFactory _experiencePieceFactory;
        private readonly IPlayerProvider _playerProvider;
        private readonly IUpdateController _updateController;

        public GameFactory(IAssetProviderService assetProviderService,
            IUpdateController updateController,
            IInputService inputService,
            IArenaService arenaService,
            IAbilityFactory abilityFactory,
            IProjectileFactory projectileFactory,
            IVfxFactory vfxFactory,
            IDataService dataService,
            IObjectContainerProvider objectContainerProvider,
            IDebugController debugController,
            IProgressService progressService,
            IExperiencePieceFactory experiencePieceFactory,
            IPlayerProvider playerProvider)
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
            _experiencePieceFactory = experiencePieceFactory;
            _playerProvider = playerProvider;
            _updateController = updateController;
        }

        public Player CreatePlayer()
        {
            var player = _assetProviderService.Instantiate<Player>(AssetPaths.PlayerPrefab);
            player.Init(_inputService, _updateController, _arenaService, _abilityFactory, _dataService, _progressService);
            return player;
        }

        public Enemy CreateEnemy(Vector3 position, EnemyData data)
        {
            // TODO: init via container
            var enemy = Object.Instantiate(data.Prefab, position, Quaternion.identity, _objectContainerProvider.EnemyContainer);
            enemy.Init(_updateController, _experiencePieceFactory, _projectileFactory, _playerProvider.Player, data, _vfxFactory,
                _debugController, _dataService);
            return enemy;
        }
    }
}