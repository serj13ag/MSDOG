using Core.Models.Data;
using Core.Services;
using Gameplay.Enemies;
using Gameplay.Providers;
using UnityEngine;
using Utility.Pools;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Factories
{
    public class GameFactory : IGameFactory
    {
        private readonly IObjectResolver _container;
        private readonly IDataService _dataService;
        private readonly IObjectContainerProvider _objectContainerProvider;

        private readonly GameObjectPoolRegistry<Enemy> _enemyPools = new();

        public GameFactory(IObjectResolver container,
            IDataService dataService,
            IObjectContainerProvider objectContainerProvider)
        {
            _container = container;
            _dataService = dataService;
            _objectContainerProvider = objectContainerProvider;
        }

        public IPlayer CreatePlayer()
        {
            var player = _container.Instantiate(_dataService.GetSettings().PlayerPrefab);
            player.Init();
            return player;
        }

        public IEnemy CreateEnemy(Vector3 position, EnemyData data)
        {
            var enemy = _enemyPools.Get(data.Prefab,
                () => _container.Instantiate(data.Prefab, _objectContainerProvider.EnemyContainer));
            enemy.Init(data, position);
            return enemy;
        }
    }
}