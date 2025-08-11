using Core.Services;
using Gameplay.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Factories
{
    public class DamageTextFactory : IDamageTextFactory
    {
        private readonly IObjectResolver _container;
        private readonly DamageTextView _damageTextViewPrefab;

        public DamageTextFactory(IObjectResolver container, IDataService dataService)
        {
            _container = container;
            _damageTextViewPrefab = dataService.GetSettingsData().DamageTextViewPrefab;
        }

        public void CreateDamageTextEffect(int damageDealt, Vector3 position)
        {
            var damageTextView = _container.Instantiate(_damageTextViewPrefab, position, Quaternion.Euler(90f, 0f, 0f));
            damageTextView.Init(damageDealt);
        }
    }
}