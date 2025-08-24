using System.Collections.Generic;
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

        private readonly Dictionary<Enemy, GameObjectPool<Enemy>> _enemyPools = new();

        public GameFactory(IObjectResolver container,
            IDataService dataService,
            IObjectContainerProvider objectContainerProvider)
        {
            _container = container;
            _dataService = dataService;
            _objectContainerProvider = objectContainerProvider;
        }

        public Player CreatePlayer()
        {
            var settingsData = _dataService.GetSettingsData();
            var player = _container.Instantiate(settingsData.PlayerPrefab);
            player.Init();
            return player;
        }

        public Enemy CreateEnemy(Vector3 position, EnemyData data)
        {
            if (!_enemyPools.ContainsKey(data.Prefab))
            {
                _enemyPools.Add(data.Prefab,
                    new GameObjectPool<Enemy>(() =>
                        _container.Instantiate(data.Prefab, position, Quaternion.identity,
                            _objectContainerProvider.EnemyContainer)));
            }

            var enemy = _enemyPools[data.Prefab].Get();
            enemy.Init(data, position);
            return enemy;
        }
    }
}