using System;
using System.Collections.Generic;
using Core.Models.Data;
using Core.Services;
using Gameplay.Projectiles;
using Gameplay.Projectiles.Views;
using Gameplay.Services;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Factories
{
    public class ProjectileFactory
    {
        private const int NumberOfPrewarmedPrefabs = 10;

        private readonly IObjectResolver _container;
        private readonly ObjectContainerService _objectContainerService;
        private readonly DataService _dataService;

        private readonly Dictionary<BaseProjectileView, ObjectPool<BaseProjectileView>> _pools = new();

        public ProjectileFactory(IObjectResolver container, ObjectContainerService objectContainerService,
            DataService dataService)
        {
            _container = container;
            _objectContainerService = objectContainerService;
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
                var createdPrefabs = new BaseProjectileView[NumberOfPrewarmedPrefabs];
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

        public void CreateAbilityProjectile(ProjectileSpawnData projectileSpawnData)
        {
            var projectile = new Projectile(projectileSpawnData, true);

            var projectileData = projectileSpawnData.ProjectileData;
            var pool = _pools[projectileData.ViewPrefab];
            var projectileView = CreateProjectileView(projectileSpawnData, projectileData, projectile, pool);
            projectileView.SetReleaseCallback(() => pool.Release(projectileView));
            projectileView.transform.position = projectileSpawnData.SpawnPosition;
            projectileView.transform.rotation = Quaternion.LookRotation(projectileSpawnData.ForwardDirection);
        }

        private BaseProjectileView CreateProjectileView(ProjectileSpawnData projectileSpawnData, ProjectileData projectileData,
            Projectile projectile, ObjectPool<BaseProjectileView> pool)
        {
            switch (projectileData.Type)
            {
                case ProjectileType.BulletHell:
                case ProjectileType.Gunshot:
                {
                    var view = (DefaultProjectileView)pool.Get();
                    view.Init(projectile, projectileSpawnData.ProjectileData);
                    return view;
                }
                case ProjectileType.BuzzSaw:
                {
                    var view = (BuzzSawProjectileView)pool.Get();
                    view.Init(projectile, projectileSpawnData.Player);
                    return view;
                }
                case ProjectileType.EnergyLine:
                {
                    var view = (EnergyLineProjectileView)pool.Get();
                    view.Init(projectile, projectileSpawnData.Player);
                    return view;
                }
                case ProjectileType.Puddle:
                {
                    var view = (PuddleProjectileView)pool.Get();
                    view.Init(projectile);
                    return view;
                }
                case ProjectileType.Enemy:
                case ProjectileType.Undefined:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void CreateEnemyProjectile(ProjectileSpawnData projectileSpawnData)
        {
            var projectile = new Projectile(projectileSpawnData, false);

            var enemyProjectileData = _dataService.GetEnemyProjectileData();

            var pool = _pools[enemyProjectileData.ViewPrefab];
            var view = (DefaultProjectileView)pool.Get();
            view.Init(projectile, enemyProjectileData);
            view.SetReleaseCallback(() => pool.Release(view));
            view.transform.position = projectileSpawnData.SpawnPosition;
            view.transform.rotation = Quaternion.LookRotation(projectileSpawnData.ForwardDirection);
        }

        private List<BaseProjectileView> GetAvailablePrefabsForLevel(int levelIndex)
        {
            var availablePrefabs = new List<BaseProjectileView>();

            var startAbilities = _dataService.GetStartAbilitiesData(levelIndex);
            foreach (var startAbility in startAbilities)
            {
                var projectileData = startAbility.ProjectileData;
                if (projectileData != null)
                {
                    availablePrefabs.Add(projectileData.ViewPrefab);
                }
            }

            var abilitiesAvailableToCraft = _dataService.GetAbilitiesAvailableToCraft(levelIndex);
            foreach (var ability in abilitiesAvailableToCraft)
            {
                var projectileData = ability.ProjectileData;
                if (projectileData != null)
                {
                    availablePrefabs.Add(projectileData.ViewPrefab);
                }
            }

            availablePrefabs.Add(_dataService.GetEnemyProjectileData().ViewPrefab);

            return availablePrefabs;
        }

        private void TryToCreatePool(BaseProjectileView projectileView)
        {
            if (!_pools.ContainsKey(projectileView))
            {
                _pools.Add(projectileView, new ObjectPool<BaseProjectileView>(
                    createFunc: () => _container.Instantiate(projectileView, _objectContainerService.ProjectileContainer),
                    actionOnGet: obj => obj.OnGet(),
                    actionOnRelease: obj => obj.OnRelease()));
            }
        }
    }
}