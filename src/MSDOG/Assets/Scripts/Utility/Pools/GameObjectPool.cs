using System;
using UnityEngine.Pool;

namespace Utility.Pools
{
    public class GameObjectPool<T> where T : BasePooledObject
    {
        private readonly ObjectPool<T> _pool;

        public GameObjectPool(Func<T> instantiate)
        {
            _pool = new ObjectPool<T>(
                createFunc: instantiate.Invoke,
                actionOnGet: obj => obj.OnGet(),
                actionOnRelease: obj => obj.OnRelease());
        }

        public T Get()
        {
            var pooledObject = _pool.Get();
            pooledObject.SetReleaseCallback(() => _pool.Release(pooledObject));
            return pooledObject;
        }

        public void Release(T pooledObject)
        {
            _pool.Release(pooledObject);
        }
    }
}