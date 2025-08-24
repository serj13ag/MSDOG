using System;
using System.Collections.Generic;

namespace Utility.Pools
{
    public class GameObjectPoolRegistry<T> where T : BasePooledObject
    {
        private readonly Dictionary<T, GameObjectPool<T>> _enemyPools = new();

        public T Get(T prefab, Func<T> instantiate)
        {
            if (!_enemyPools.ContainsKey(prefab))
            {
                _enemyPools.Add(prefab, new GameObjectPool<T>(instantiate.Invoke));
            }

            var pooledObject = _enemyPools[prefab].Get();
            return pooledObject;
        }

        public void Prewarm(T prefab, Func<T> instantiate, int numberOfPrewarmedPrefabs)
        {
            var createdPrefabs = new T[numberOfPrewarmedPrefabs];

            for (var i = 0; i < numberOfPrewarmedPrefabs; i++)
            {
                createdPrefabs[i] = Get(prefab, instantiate);
            }

            foreach (var createdPrefab in createdPrefabs)
            {
                _enemyPools[prefab].Release(createdPrefab);
            }
        }
    }
}