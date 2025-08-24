using System;
using System.Collections.Generic;
using Core.Models.Data;
using Core.Services;
using Gameplay.Abilities;
using Gameplay.AbilityEffects;
using Gameplay.Providers;
using UnityEngine;
using Utility.Pools;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Factories
{
    public class AbilityEffectFactory : IAbilityEffectFactory
    {
        private const int NumberOfPrewarmedPrefabs = 5;

        private readonly IObjectResolver _container;
        private readonly IObjectContainerProvider _objectContainerProvider;
        private readonly IDataService _dataService;

        private readonly GameObjectPoolRegistry<BaseAbilityEffect> _pools = new();

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
                _pools.Prewarm(availablePrefab, Instantiate(availablePrefab), NumberOfPrewarmedPrefabs);
            }
        }

        public T CreateEffect<T>(Player player, AbilityData abilityData) where T : BaseAbilityEffect
        {
            var prefab = abilityData.FollowingAbilityEffectPrefab;
            var baseAbilityEffect = (T)_pools.Get(prefab, Instantiate(prefab));

            switch (baseAbilityEffect)
            {
                case FollowingAbilityEffect followingAbilityEffect:
                    followingAbilityEffect.Init(player.transform);
                    break;
                case OneTimeAbilityEffect oneTimeAbilityEffect:
                    var position = player.GetAbilitySpawnPosition(abilityData.AbilityType);
                    var rotation = Quaternion.Euler(90f, 0f, 0f);
                    var scale = GetScale(abilityData);
                    oneTimeAbilityEffect.Init(position, rotation, scale);
                    break;
            }

            return baseAbilityEffect;
        }

        private List<BaseAbilityEffect> GetAvailablePrefabsForLevel(int levelIndex)
        {
            var availablePrefabs = new List<BaseAbilityEffect>();

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

        private Func<BaseAbilityEffect> Instantiate(BaseAbilityEffect prefab)
        {
            return () => _container.Instantiate(prefab, _objectContainerProvider.AbilityEffectContainer);
        }

        /// <summary>
        /// Visual scale base on effect size
        /// </summary>
        private static Vector3 GetScale(AbilityData abilityData)
        {
            switch (abilityData.AbilityType)
            {
                case AbilityType.CuttingBlow:
                {
                    var size = abilityData.Size;
                    var t = Mathf.InverseLerp(7f, 9f, size);
                    var scale = Mathf.LerpUnclamped(0.7f, 1f, t);
                    return new Vector3(scale, 1.4f, 1f);
                }
                case AbilityType.RoundAttack:
                {
                    var size = abilityData.Size;
                    var t = Mathf.InverseLerp(3f, 6f, size);
                    var scale = Mathf.LerpUnclamped(1f, 2.2f, t);
                    return new Vector3(scale, scale, 1f);
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}