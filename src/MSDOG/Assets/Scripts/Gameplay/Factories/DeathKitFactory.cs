using System.Collections.Generic;
using Core.Services;
using Gameplay.Enemies;
using Gameplay.Services;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.Factories
{
    public class DeathKitFactory
    {
        private const int NumberOfPrewarmedPrefabs = 10;

        private readonly ObjectContainerService _objectContainerService;
        private readonly IDataService _dataService;

        private readonly Dictionary<EnemyDeathkit, ObjectPool<EnemyDeathkit>> _pools = new();

        public DeathKitFactory(ObjectContainerService objectContainerService, IDataService dataService)
        {
            _objectContainerService = objectContainerService;
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
                    if (!_pools.ContainsKey(deathKitPrefab))
                    {
                        _pools.Add(deathKitPrefab, new ObjectPool<EnemyDeathkit>(
                            createFunc: () => Object.Instantiate(deathKitPrefab, _objectContainerService.DeathKitContainer),
                            actionOnGet: obj => obj.OnGet(),
                            actionOnRelease: obj => obj.OnRelease()));
                    }
                }
            }

            foreach (var pool in _pools.Values)
            {
                var createdPrefabs = new EnemyDeathkit[NumberOfPrewarmedPrefabs];
                for (var i = 0; i < NumberOfPrewarmedPrefabs; i++)
                {
                    createdPrefabs[i] = pool.Get();
                }

                foreach (var prefab in createdPrefabs)
                {
                    pool.Release(prefab);
                }
            }
        }

        public EnemyDeathkit CreateDeathKit(EnemyDeathkit deathKitPrefab, Vector3 position, Quaternion rotation)
        {
            var pool = _pools[deathKitPrefab];
            var enemyDeathKit = pool.Get();
            enemyDeathKit.Init(position, rotation);
            enemyDeathKit.SetReleaseCallback(() => pool.Release(enemyDeathKit));
            return enemyDeathKit;
        }
    }
}