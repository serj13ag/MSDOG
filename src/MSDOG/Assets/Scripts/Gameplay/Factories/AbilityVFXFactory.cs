using System;
using System.Collections.Generic;
using Core.Models.Data;
using Core.Services;
using Gameplay.Abilities.Core;
using Gameplay.Abilities.View.VFX;
using Gameplay.Interfaces;
using Gameplay.Providers;
using UnityEngine;
using Utility.Pools;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Factories
{
    public class AbilityVFXFactory : IAbilityVFXFactory
    {
        private const int NumberOfPrewarmedPrefabs = 5;

        private readonly IObjectResolver _container;
        private readonly IObjectContainerProvider _objectContainerProvider;
        private readonly IDataService _dataService;

        private readonly GameObjectPoolRegistry<BaseAbilityVFX> _pools = new();

        public AbilityVFXFactory(IObjectResolver container, IObjectContainerProvider objectContainerProvider,
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

        public T CreateEffect<T>(IEntityWithAbilities entityWithAbilities, AbilityData abilityData) where T : BaseAbilityVFX
        {
            var prefab = abilityData.FollowingAbilityVFXPrefab;
            var baseAbilityVFX = (T)_pools.Get(prefab, Instantiate(prefab));

            switch (baseAbilityVFX)
            {
                case FollowingAbilityVFX followingAbilityVFX:
                    followingAbilityVFX.Init(entityWithAbilities);
                    break;
                case OneTimeAbilityVFX oneTimeAbilityVFX:
                    var position = entityWithAbilities.GetAbilitySpawnPosition(abilityData.AbilityType);
                    var rotation = Quaternion.Euler(90f, 0f, 0f);
                    var scale = GetScale(abilityData);
                    oneTimeAbilityVFX.Init(position, rotation, scale);
                    break;
            }

            return baseAbilityVFX;
        }

        private List<BaseAbilityVFX> GetAvailablePrefabsForLevel(int levelIndex)
        {
            var availablePrefabs = new List<BaseAbilityVFX>();

            var startAbilities = _dataService.GetStartAbilitiesData(levelIndex);
            foreach (var startAbility in startAbilities)
            {
                var followingAbilityVFXPrefab = startAbility.FollowingAbilityVFXPrefab;
                if (followingAbilityVFXPrefab)
                {
                    availablePrefabs.Add(followingAbilityVFXPrefab);
                }
            }

            var abilitiesAvailableToCraft = _dataService.GetAbilitiesAvailableToCraft(levelIndex);
            foreach (var ability in abilitiesAvailableToCraft)
            {
                var followingAbilityVFXPrefab = ability.FollowingAbilityVFXPrefab;
                if (followingAbilityVFXPrefab)
                {
                    availablePrefabs.Add(followingAbilityVFXPrefab);
                }
            }

            return availablePrefabs;
        }

        private Func<BaseAbilityVFX> Instantiate(BaseAbilityVFX prefab)
        {
            return () => _container.Instantiate(prefab, _objectContainerProvider.AbilityVFXContainer);
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