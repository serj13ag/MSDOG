using Core.Services;
using Gameplay.Providers;
using Gameplay.UI;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Factories
{
    // TODO: add base class
    public class DamageTextFactory : IDamageTextFactory
    {
        private const int NumberOfPrewarmedPrefabs = 20;

        private readonly ObjectPool<DamageTextView> _pool;

        public DamageTextFactory(IObjectResolver container, IDataService dataService,
            IObjectContainerProvider objectContainerProvider)
        {
            var settingsData = dataService.GetSettingsData();

            _pool = new ObjectPool<DamageTextView>(
                createFunc: () => container.Instantiate(settingsData.DamageTextViewPrefab,
                    objectContainerProvider.DamageTextContainer),
                actionOnGet: obj => obj.OnGet(),
                actionOnRelease: obj => obj.OnRelease());
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
            damageTextView.SetReleaseCallback(() => _pool.Release(damageTextView));
            damageTextView.Init(position, damageDealt);
        }
    }
}