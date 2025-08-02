using System;
using Constants;
using Core.Projectiles;
using Core.Projectiles.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Services.Gameplay
{
    public class ProjectileFactory
    {
        private readonly IObjectResolver _container;
        private readonly AssetProviderService _assetProviderService;
        private readonly RuntimeContainers _runtimeContainers;
        private readonly DataService _dataService;

        public ProjectileFactory(IObjectResolver container, AssetProviderService assetProviderService,
            RuntimeContainers runtimeContainers, DataService dataService)
        {
            _container = container;
            _assetProviderService = assetProviderService;
            _runtimeContainers = runtimeContainers;
            _dataService = dataService;
        }

        public void CreateAbilityProjectile(ProjectileSpawnData projectileSpawnData)
        {
            var projectile = new Projectile(projectileSpawnData, true);

            var projectileData = projectileSpawnData.ProjectileData;
            switch (projectileData.Type)
            {
                case ProjectileType.BulletHell:
                case ProjectileType.Gunshot:
                {
                    var prefab = (DefaultProjectileView)projectileData.ViewPrefab;
                    var view = _container.Instantiate(prefab, projectileSpawnData.SpawnPosition,
                        Quaternion.LookRotation(projectileSpawnData.ForwardDirection));
                    view.Init(projectile, projectileSpawnData.ProjectileData);
                    break;
                }
                case ProjectileType.BuzzSaw:
                {
                    var viewPrefab = (BuzzSawProjectileView)projectileData.ViewPrefab;
                    var view = _container.Instantiate(viewPrefab, projectileSpawnData.SpawnPosition,
                        Quaternion.LookRotation(projectileSpawnData.ForwardDirection));
                    view.Init(projectile, projectileSpawnData.Player);
                    break;
                }
                case ProjectileType.EnergyLine:
                {
                    var viewPrefab = (EnergyLineProjectileView)projectileData.ViewPrefab;
                    var view = _container.Instantiate(viewPrefab, projectileSpawnData.SpawnPosition,
                        Quaternion.LookRotation(projectileSpawnData.ForwardDirection));
                    view.Init(projectile, projectileSpawnData.Player);
                    break;
                }
                case ProjectileType.Puddle:
                {
                    var viewPrefab = (PuddleProjectileView)projectileData.ViewPrefab;
                    var view = _container.Instantiate(viewPrefab, projectileSpawnData.SpawnPosition,
                        Quaternion.LookRotation(projectileSpawnData.ForwardDirection));
                    view.Init(projectile);
                    break;
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
            var projectileView = _assetProviderService.Instantiate<DefaultProjectileView>(AssetPaths.EnemyProjectilePrefab,
                projectileSpawnData.SpawnPosition, Quaternion.LookRotation(projectileSpawnData.ForwardDirection),
                _runtimeContainers.ProjectileContainer,
                _container);
            projectileView.Init(projectile, _dataService.GetEnemyProjectileData());
        }
    }
}