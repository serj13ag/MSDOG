using System;
using System.Collections.Generic;
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
        private readonly IDataService _dataService;
        private readonly PlayerService _playerService;

        private readonly Dictionary<BaseProjectileView, ObjectPool<BaseProjectileView>> _pools = new();

        public ProjectileFactory(IObjectResolver container, ObjectContainerService objectContainerService,
            IDataService dataService, PlayerService playerService)
        {
            _container = container;
            _objectContainerService = objectContainerService;
            _dataService = dataService;
            _playerService = playerService;
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
            var pool = _pools[projectileSpawnData.ProjectileData.ViewPrefab];
            var view = pool.Get();
            view.SetReleaseCallback(() => pool.Release(view));
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
                    buzzSawProjectileView.Init(projectile, _playerService.Player);
                    break;
                }
                case ProjectileType.EnergyLine:
                {
                    var energyLineProjectileView = (EnergyLineProjectileView)view;
                    energyLineProjectileView.Init(projectile, _playerService.Player);
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