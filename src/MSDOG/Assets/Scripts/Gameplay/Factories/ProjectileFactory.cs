using System;
using System.Collections.Generic;
using Core.Services;
using Gameplay.Projectiles;
using Gameplay.Projectiles.Views;
using Gameplay.Providers;
using UnityEngine;
using Utility.Pools;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Factories
{
    public class ProjectileFactory : IProjectileFactory
    {
        private const int NumberOfPrewarmedPrefabs = 10;

        private readonly IObjectResolver _container;
        private readonly IObjectContainerProvider _objectContainerProvider;
        private readonly IDataService _dataService;
        private readonly IPlayerProvider _playerProvider;

        private readonly GameObjectPoolRegistry<BaseProjectileView> _pools = new();

        public ProjectileFactory(IObjectResolver container, IObjectContainerProvider objectContainerProvider,
            IDataService dataService, IPlayerProvider playerProvider)
        {
            _container = container;
            _objectContainerProvider = objectContainerProvider;
            _dataService = dataService;
            _playerProvider = playerProvider;
        }

        public void Prewarm(int levelIndex)
        {
            var availablePrefabs = GetAvailablePrefabsForLevel(levelIndex);
            foreach (var availablePrefab in availablePrefabs)
            {
                var createdPrefabs = new BaseProjectileView[NumberOfPrewarmedPrefabs];

                for (var i = 0; i < NumberOfPrewarmedPrefabs; i++)
                {
                    createdPrefabs[i] = _pools.Get(availablePrefab, Instantiate(availablePrefab));
                }

                foreach (var prefab in createdPrefabs)
                {
                    _pools.Release(availablePrefab, prefab);
                }
            }
        }

        public void CreateAbilityProjectile(ProjectileSpawnData projectileSpawnData)
        {
            var projectile = new Projectile(projectileSpawnData, true);

            var view = GetFromPool(projectileSpawnData);
            InitializeView(view, projectile, projectileSpawnData);
        }

        public void CreateEnemyProjectile(ProjectileSpawnData projectileSpawnData)
        {
            var projectile = new Projectile(projectileSpawnData, false);

            var view = GetFromPool(projectileSpawnData);
            InitializeView(view, projectile, projectileSpawnData);
        }

        private BaseProjectileView GetFromPool(ProjectileSpawnData projectileSpawnData)
        {
            var projectileViewPrefab = projectileSpawnData.ProjectileData.ViewPrefab;
            var view = _pools.Get(projectileViewPrefab, Instantiate(projectileViewPrefab));
            view.transform.position = projectileSpawnData.SpawnPosition;
            view.transform.rotation = Quaternion.LookRotation(projectileSpawnData.ForwardDirection);
            return view;
        }

        private void InitializeView(BaseProjectileView view, Projectile projectile, ProjectileSpawnData projectileSpawnData)
        {
            switch (projectileSpawnData.ProjectileData.Type)
            {
                case ProjectileType.BulletHell:
                case ProjectileType.Gunshot:
                case ProjectileType.Enemy:
                {
                    var defaultProjectileView = (DefaultProjectileView)view;
                    defaultProjectileView.Init(projectile, projectileSpawnData.ProjectileData);
                    break;
                }
                case ProjectileType.BuzzSaw:
                {
                    var buzzSawProjectileView = (BuzzSawProjectileView)view;
                    buzzSawProjectileView.Init(projectile, _playerProvider.Player);
                    break;
                }
                case ProjectileType.EnergyLine:
                {
                    var energyLineProjectileView = (EnergyLineProjectileView)view;
                    energyLineProjectileView.Init(projectile, _playerProvider.Player);
                    break;
                }
                case ProjectileType.Puddle:
                {
                    var puddleProjectileView = (PuddleProjectileView)view;
                    puddleProjectileView.Init(projectile);
                    break;
                }
                case ProjectileType.Undefined:
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

        private Func<BaseProjectileView> Instantiate(BaseProjectileView availablePrefab)
        {
            return () => _container.Instantiate(availablePrefab, _objectContainerProvider.ProjectileContainer);
        }
    }
}