using Core.Controllers;
using Core.Models.Data;
using Core.Services;
using Gameplay.Enemies;
using Gameplay.Providers;
using Gameplay.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Factories
{
    public class GameFactory : IGameFactory
    {
        private readonly IObjectResolver _container;
        private readonly IInputService _inputService;
        private readonly IArenaService _arenaService;
        private readonly IAbilityFactory _abilityFactory;
        private readonly IDataService _dataService;
        private readonly IObjectContainerProvider _objectContainerProvider;
        private readonly IProgressService _progressService;
        private readonly IUpdateController _updateController;

        public GameFactory(IObjectResolver container,
            IUpdateController updateController,
            IInputService inputService,
            IArenaService arenaService,
            IAbilityFactory abilityFactory,
            IDataService dataService,
            IObjectContainerProvider objectContainerProvider,
            IProgressService progressService)
        {
            _container = container;
            _inputService = inputService;
            _arenaService = arenaService;
            _abilityFactory = abilityFactory;
            _dataService = dataService;
            _objectContainerProvider = objectContainerProvider;
            _progressService = progressService;
            _updateController = updateController;
        }

        public Player CreatePlayer()
        {
            var settingsData = _dataService.GetSettingsData();
            var player = _container.Instantiate(settingsData.PlayerPrefab);
            player.Init(_inputService, _updateController, _arenaService, _abilityFactory, _dataService, _progressService);
            return player;
        }

        public Enemy CreateEnemy(Vector3 position, EnemyData data)
        {
            var enemy = _container.Instantiate(data.Prefab, position, Quaternion.identity,
                _objectContainerProvider.EnemyContainer);
            enemy.Init(data);
            return enemy;
        }
    }
}