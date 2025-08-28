using Core.Services;
using Gameplay.Providers;
using Gameplay.UI;
using UnityEngine;
using Utility.Pools;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Factories
{
    public class DamageTextFactory : IDamageTextFactory
    {
        private const int NumberOfPrewarmedPrefabs = 20;

        private readonly GameObjectPool<DamageTextView> _pool;

        public DamageTextFactory(IObjectResolver container, IDataService dataService,
            IObjectContainerProvider objectContainerProvider)
        {
            _pool = new GameObjectPool<DamageTextView>(() =>
                container.Instantiate(dataService.GetSettings().DamageTextViewPrefab,
                    objectContainerProvider.DamageTextContainer));
        }

        public void Prewarm()
        {
            var createdPrefabs = new DamageTextView[NumberOfPrewarmedPrefabs];
            for (var i = 0; i < NumberOfPrewarmedPrefabs; i++)
            {
                createdPrefabs[i] = _pool.Get();
            }

            foreach (var prefab in createdPrefabs)
            {
                _pool.Release(prefab);
            }
        }

        public void CreateDamageTextEffect(int damageDealt, Vector3 position)
        {
            var damageTextView = _pool.Get();
            damageTextView.Init(position, damageDealt);
        }
    }
}