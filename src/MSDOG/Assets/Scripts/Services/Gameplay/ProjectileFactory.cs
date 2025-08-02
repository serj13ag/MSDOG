using Constants;
using Core.Projectiles;
using DTO;
using UnityEngine;
using VContainer;

namespace Services.Gameplay
{
    public class ProjectileFactory
    {
        private readonly Vector3 _playerProjectileOffset = Vector3.up * 1f;
        private readonly Vector3 _enemyProjectileOffset = Vector3.up * 0.8f;

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

        public void CreatePlayerGunShotProjectile(CreateProjectileDto createProjectileDto)
        {
            CreateProjectileInner(AssetPaths.PlayerGunShotProjectilePrefab,
                createProjectileDto.SpawnPosition + _playerProjectileOffset,
                Quaternion.LookRotation(createProjectileDto.ForwardDirection),
                createProjectileDto, ProjectileType.Gunshot);
        }

        public void CreatePlayerBulletHellProjectile(CreateProjectileDto createProjectileDto)
        {
            CreateProjectileInner(AssetPaths.PlayerBulletHellProjectilePrefab,
                createProjectileDto.SpawnPosition + _playerProjectileOffset,
                Quaternion.LookRotation(createProjectileDto.ForwardDirection),
                createProjectileDto, ProjectileType.BulletHell);
        }

        public void CreateEnemyProjectile(CreateProjectileDto createProjectileDto)
        {
            CreateProjectileInner(AssetPaths.EnemyProjectilePrefab,
                createProjectileDto.SpawnPosition + _enemyProjectileOffset,
                Quaternion.LookRotation(createProjectileDto.ForwardDirection),
                createProjectileDto, ProjectileType.Enemy);
        }

        public void CreatePlayerBuzzSawProjectile(CreateProjectileDto createProjectileDto)
        {
            var projectile = _assetProviderService.Instantiate<BuzzSawProjectile>(AssetPaths.PlayerBuzzSawProjectilePrefab,
                createProjectileDto.SpawnPosition + _playerProjectileOffset, Quaternion.identity,
                _runtimeContainers.ProjectileContainer, _container);
            var projectileCore = new ProjectileCore(createProjectileDto, ProjectileType.BuzzSaw);
            projectile.Init(projectileCore, createProjectileDto.Player);
        }

        public void CreatePlayerPuddleProjectile(CreateProjectileDto createProjectileDto)
        {
            var projectile = _assetProviderService.Instantiate<PuddleProjectile>(AssetPaths.PlayerPuddleProjectilePrefab,
                createProjectileDto.SpawnPosition, Quaternion.identity, _runtimeContainers.ProjectileContainer, _container);
            var projectileCore = new ProjectileCore(createProjectileDto, ProjectileType.Puddle);
            projectile.Init(projectileCore);
        }

        public void CreatePlayerEnergyLineProjectile(CreateProjectileDto createProjectileDto)
        {
            var projectile = _assetProviderService.Instantiate<EnergyLineProjectile>(AssetPaths.PlayerEnergyLineProjectilePrefab,
                createProjectileDto.SpawnPosition + _playerProjectileOffset, Quaternion.identity,
                _runtimeContainers.ProjectileContainer, _container);
            var projectileCore = new ProjectileCore(createProjectileDto, ProjectileType.EnergyLine);
            projectile.Init(createProjectileDto, projectileCore);
        }

        private void CreateProjectileInner(string prefabPath, Vector3 position, Quaternion rotation,
            CreateProjectileDto createProjectileDto, ProjectileType projectileType)
        {
            var projectile = new ProjectileCore(createProjectileDto, projectileType);
            var projectileView = _assetProviderService.Instantiate<Projectile>(prefabPath, position, rotation,
                _runtimeContainers.ProjectileContainer, _container);
            projectileView.Init(projectile);
        }
    }
}