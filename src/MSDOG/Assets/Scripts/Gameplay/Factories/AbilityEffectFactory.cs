using System.Collections.Generic;
using Core.Services;
using Gameplay.Providers;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Factories
{
    // TODO: create base mult factory
    public class AbilityEffectFactory : IAbilityEffectFactory
    {
        private const int NumberOfPrewarmedPrefabs = 5;

        private readonly IObjectResolver _container;
        private readonly IObjectContainerProvider _objectContainerProvider;
        private readonly IDataService _dataService;

        private readonly Dictionary<FollowingAbilityEffect, ObjectPool<FollowingAbilityEffect>> _pools = new();

        public AbilityEffectFactory(IObjectResolver container, IObjectContainerProvider objectContainerProvider,
            IDataService dataService)
        {
            _container = container;
            _objectContainerProvider = objectContainerProvider;
            _dataService = dataService;
        }

        public void Prewarm(int levelIndex)
        {
            var availablePrefabs = GetAvailablePrefabsForLevel(levelIndex);
            foreach (var availablePrefab in availablePrefabs)
            {
                TryToCreatePool(availablePrefab);
            }

            foreach (var pool in _pools.Values)
            {
                var createdPrefabs = new FollowingAbilityEffect[NumberOfPrewarmedPrefabs];
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

        public FollowingAbilityEffect CreateFollowingEffect(FollowingAbilityEffect abilityEffectPrefab, Player player)
        {
            var pool = _pools[abilityEffectPrefab];
            var followingAbilityEffect = pool.Get();
            followingAbilityEffect.SetReleaseCallback(() => pool.Release(followingAbilityEffect));
            followingAbilityEffect.Init(player.transform);
            return followingAbilityEffect;
        }

        private List<FollowingAbilityEffect> GetAvailablePrefabsForLevel(int levelIndex)
        {
            var availablePrefabs = new List<FollowingAbilityEffect>();

            var startAbilities = _dataService.GetStartAbilitiesData(levelIndex);
            foreach (var startAbility in startAbilities)
            {
                var followingAbilityEffectPrefab = startAbility.FollowingAbilityEffectPrefab;
                if (followingAbilityEffectPrefab)
                {
                    availablePrefabs.Add(followingAbilityEffectPrefab);
                }
            }

            var abilitiesAvailableToCraft = _dataService.GetAbilitiesAvailableToCraft(levelIndex);
            foreach (var ability in abilitiesAvailableToCraft)
            {
                var followingAbilityEffectPrefab = ability.FollowingAbilityEffectPrefab;
                if (followingAbilityEffectPrefab)
                {
                    availablePrefabs.Add(followingAbilityEffectPrefab);
                }
            }

            return availablePrefabs;
        }

        private void TryToCreatePool(FollowingAbilityEffect projectileView)
        {
            if (!_pools.ContainsKey(projectileView))
            {
                _pools.Add(projectileView, new ObjectPool<FollowingAbilityEffect>(
                    createFunc: () => _container.Instantiate(projectileView, _objectContainerProvider.AbilityEffectContainer),
                    actionOnGet: obj => obj.OnGet(),
                    actionOnRelease: obj => obj.OnRelease()));
            }
        }
    }
}