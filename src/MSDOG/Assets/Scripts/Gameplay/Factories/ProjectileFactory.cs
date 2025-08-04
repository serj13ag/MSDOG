using System;
using Core.Services;
using Gameplay.Projectiles;
using Gameplay.Projectiles.Views;
using Gameplay.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Factories
{
    public class ProjectileFactory
    {
        private readonly IObjectResolver _container;
        private readonly RuntimeContainers _runtimeContainers;
        private readonly DataService _dataService;

        public ProjectileFactory(IObjectResolver container, RuntimeContainers runtimeContainers, DataService dataService)
        {
            _container = container;
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
                    var viewPrefab = (DefaultProjectileView)projectileData.ViewPrefab;
                    var view = InstantiateView(viewPrefab, projectileSpawnData.SpawnPosition,
                        projectileSpawnData.ForwardDirection);
                    view.Init(projectile, projectileSpawnData.ProjectileData);
                    break;
                }
                case ProjectileType.BuzzSaw:
                {
                    var viewPrefab = (BuzzSawProjectileView)projectileData.ViewPrefab;
                    var view = InstantiateView(viewPrefab, projectileSpawnData.SpawnPosition,
                        projectileSpawnData.ForwardDirection);
                    view.Init(projectile, projectileSpawnData.Player);
                    break;
                }
                case ProjectileType.EnergyLine:
                {
                    var viewPrefab = (EnergyLineProjectileView)projectileData.ViewPrefab;
                    var view = InstantiateView(viewPrefab, projectileSpawnData.SpawnPosition,
                        projectileSpawnData.ForwardDirection);
                    view.Init(projectile, projectileSpawnData.Player);
                    break;
                }
                case ProjectileType.Puddle:
                {
                    var viewPrefab = (PuddleProjectileView)projectileData.ViewPrefab;
                    var view = InstantiateView(viewPrefab, projectileSpawnData.SpawnPosition,
                        projectileSpawnData.ForwardDirection);
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

            var enemyProjectileData = _dataService.GetEnemyProjectileData();

            var viewPrefab = (DefaultProjectileView)enemyProjectileData.ViewPrefab;
            var view = InstantiateView(viewPrefab, projectileSpawnData.SpawnPosition, projectileSpawnData.ForwardDirection);
            view.Init(projectile, enemyProjectileData);
        }

        private T InstantiateView<T>(T prefab, Vector3 position, Vector3 forwardDirection) where T : BaseProjectileView
        {
            return _container.Instantiate(prefab, position, Quaternion.LookRotation(forwardDirection),
                _runtimeContainers.ProjectileContainer);
        }
    }
}