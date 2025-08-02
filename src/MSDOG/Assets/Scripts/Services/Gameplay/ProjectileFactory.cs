using Constants;
using Core;
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
            var projectile = _assetProviderService.Instantiate<Projectile>(AssetPaths.PlayerGunShotProjectilePrefab,
                createProjectileDto.SpawnPosition + _playerProjectileOffset,
                Quaternion.LookRotation(createProjectileDto.ForwardDirection), _runtimeContainers.ProjectileContainer,
                _container);
            projectile.Init(createProjectileDto, ProjectileType.Gunshot);
        }

        public void CreatePlayerBulletHellProjectile(CreateProjectileDto createProjectileDto)
        {
            var projectile = _assetProviderService.Instantiate<Projectile>(AssetPaths.PlayerBulletHellProjectilePrefab,
                createProjectileDto.SpawnPosition + _playerProjectileOffset,
                Quaternion.LookRotation(createProjectileDto.ForwardDirection), _runtimeContainers.ProjectileContainer,
                _container);
            projectile.Init(createProjectileDto, ProjectileType.BulletHell);
        }

        public void CreateEnemyProjectile(CreateEnemyProjectileDto createProjectileDto)
        {
            var projectile = _assetProviderService.Instantiate<Projectile>(AssetPaths.EnemyProjectilePrefab,
                createProjectileDto.SpawnPosition + _enemyProjectileOffset,
                Quaternion.LookRotation(createProjectileDto.ForwardDirection), _runtimeContainers.ProjectileContainer,
                _container);
            projectile.Init(createProjectileDto, ProjectileType.Enemy);
        }

        public void CreatePlayerBuzzSawProjectile(CreateProjectileDto createProjectileDto)
        {
            var projectile = _assetProviderService.Instantiate<BuzzSawProjectile>(AssetPaths.PlayerBuzzSawProjectilePrefab,
                createProjectileDto.SpawnPosition + _playerProjectileOffset, Quaternion.identity,
                _runtimeContainers.ProjectileContainer, _container);
            projectile.Init(createProjectileDto, true);
        }

        public void CreatePlayerPuddleProjectile(CreateProjectileDto createProjectileDto)
        {
            var projectile = _assetProviderService.Instantiate<PuddleProjectile>(AssetPaths.PlayerPuddleProjectilePrefab,
                createProjectileDto.SpawnPosition, Quaternion.identity, _runtimeContainers.ProjectileContainer, _container);
            projectile.Init(createProjectileDto);
        }

        public void CreatePlayerEnergyLineProjectile(CreateProjectileDto createProjectileDto)
        {
            var projectile = _assetProviderService.Instantiate<EnergyLineProjectile>(AssetPaths.PlayerEnergyLineProjectilePrefab,
                createProjectileDto.SpawnPosition + _playerProjectileOffset, Quaternion.identity,
                _runtimeContainers.ProjectileContainer, _container);
            projectile.Init(createProjectileDto);
        }
    }
}