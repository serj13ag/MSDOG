using System;
using Core.Services;
using Gameplay.Enemies;
using Gameplay.Providers;
using UnityEngine;
using Utility.Pools;
using Object = UnityEngine.Object;

namespace Gameplay.Factories
{
    public class DeathKitFactory : IDeathKitFactory
    {
        private const int NumberOfPrewarmedPrefabs = 10;

        private readonly IObjectContainerProvider _objectContainerProvider;
        private readonly IDataService _dataService;

        private readonly GameObjectPoolRegistry<EnemyDeathkit> _pools = new();

        public DeathKitFactory(IObjectContainerProvider objectContainerProvider, IDataService dataService)
        {
            _objectContainerProvider = objectContainerProvider;
            _dataService = dataService;
        }

        public void Prewarm(int levelIndex)
        {
            var levelData = _dataService.GetLevelData(levelIndex);
            foreach (var wave in levelData.Waves)
            {
                foreach (var enemy in wave.Enemies)
                {
                    var deathKitPrefab = enemy.Data.DeathkitPrefab;
                    _pools.Prewarm(deathKitPrefab, Instantiate(deathKitPrefab), NumberOfPrewarmedPrefabs);
                }
            }
        }

        public EnemyDeathkit CreateDeathKit(EnemyDeathkit deathKitPrefab, Vector3 position, Quaternion rotation)
        {
            var enemyDeathKit = _pools.Get(deathKitPrefab, Instantiate(deathKitPrefab));
            enemyDeathKit.Init(position, rotation);
            return enemyDeathKit;
        }

        private Func<EnemyDeathkit> Instantiate(EnemyDeathkit deathKitPrefab)
        {
            return () => Object.Instantiate(deathKitPrefab, _objectContainerProvider.DeathKitContainer);
        }
    }
}