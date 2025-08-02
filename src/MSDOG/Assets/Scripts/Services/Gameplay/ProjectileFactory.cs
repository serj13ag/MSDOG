using Constants;
using Core.Projectiles;
using UnityEngine;
using VContainer;

namespace Services.Gameplay
{
    public class ProjectileFactory
    {
        private readonly IObjectResolver _container;
        private readonly AssetProviderService _assetProviderService;
        private readonly RuntimeContainers _runtimeContainers;

        public ProjectileFactory(IObjectResolver container, AssetProviderService assetProviderService,
            RuntimeContainers runtimeContainers)
        {
            _container = container;
            _assetProviderService = assetProviderService;
            _runtimeContainers = runtimeContainers;
        }

        public void CreatePlayerGunShotProjectile(ProjectileSpawnData projectileSpawnData)
        {
            CreateProjectileViewInner(AssetPaths.PlayerGunShotProjectilePrefab,
                projectileSpawnData.SpawnPosition, Quaternion.LookRotation(projectileSpawnData.ForwardDirection),
                projectileSpawnData, ProjectileType.Gunshot);
        }

        public void CreatePlayerBulletHellProjectile(ProjectileSpawnData projectileSpawnData)
        {
            CreateProjectileViewInner(AssetPaths.PlayerBulletHellProjectilePrefab,
                projectileSpawnData.SpawnPosition, Quaternion.LookRotation(projectileSpawnData.ForwardDirection),
                projectileSpawnData, ProjectileType.BulletHell);
        }

        public void CreateEnemyProjectile(ProjectileSpawnData projectileSpawnData)
        {
            CreateProjectileViewInner(AssetPaths.EnemyProjectilePrefab,
                projectileSpawnData.SpawnPosition, Quaternion.LookRotation(projectileSpawnData.ForwardDirection),
                projectileSpawnData, ProjectileType.Enemy);
        }

        public BuzzSawProjectileView CreatePlayerBuzzSawProjectile(ProjectileSpawnData projectileSpawnData)
        {
            var projectileCore = CreateProjectile(projectileSpawnData, ProjectileType.BuzzSaw);
            var projectileView = CreateProjectileViewInner<BuzzSawProjectileView>(AssetPaths.PlayerBuzzSawProjectilePrefab,
                projectileSpawnData.SpawnPosition, Quaternion.identity);
            projectileView.Init(projectileCore, projectileSpawnData.Player);
            return projectileView;
        }

        public PuddleProjectileView CreatePlayerPuddleProjectile(ProjectileSpawnData projectileSpawnData)
        {
            var projectile = CreateProjectile(projectileSpawnData, ProjectileType.Puddle);
            var projectileView = CreateProjectileViewInner<PuddleProjectileView>(AssetPaths.PlayerPuddleProjectilePrefab,
                projectileSpawnData.SpawnPosition, Quaternion.identity);
            projectileView.Init(projectile);
            return projectileView;
        }

        public EnergyLineProjectileView CreatePlayerEnergyLineProjectile(ProjectileSpawnData projectileSpawnData)
        {
            var projectile = CreateProjectile(projectileSpawnData, ProjectileType.EnergyLine);
            var projectileView = CreateProjectileViewInner<EnergyLineProjectileView>(AssetPaths.PlayerEnergyLineProjectilePrefab,
                projectileSpawnData.SpawnPosition, Quaternion.identity);
            projectileView.Init(projectile, projectileSpawnData.Player);
            return projectileView;
        }

        private static Projectile CreateProjectile(ProjectileSpawnData projectileSpawnData, ProjectileType projectileType)
        {
            return new Projectile(projectileSpawnData, projectileType);
        }

        private ProjectileView CreateProjectileViewInner(string prefabPath, Vector3 position, Quaternion rotation,
            ProjectileSpawnData projectileSpawnData, ProjectileType projectileType)
        {
            var projectile = CreateProjectile(projectileSpawnData, projectileType);
            var projectileView = CreateProjectileViewInner<ProjectileView>(prefabPath, position, rotation);
            projectileView.Init(projectile);
            return projectileView;
        }

        private T CreateProjectileViewInner<T>(string prefabPath, Vector3 position, Quaternion rotation) where T : Component
        {
            return _assetProviderService.Instantiate<T>(prefabPath, position, rotation, _runtimeContainers.ProjectileContainer,
                _container);
        }
    }
}