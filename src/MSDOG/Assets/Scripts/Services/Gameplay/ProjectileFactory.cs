using Constants;
using Core;
using DTO;
using UnityEngine;

namespace Services.Gameplay
{
    public class ProjectileFactory
    {
        private readonly Vector3 _playerProjectileOffset = Vector3.up * 1f;
        private readonly Vector3 _enemyProjectileOffset = Vector3.up * 0.8f;

        private readonly AssetProviderService _assetProviderService;
        private readonly UpdateService _updateService;
        private readonly VfxFactory _vfxFactory;

        public ProjectileFactory(AssetProviderService assetProviderService, UpdateService updateService, VfxFactory vfxFactory)
        {
            _assetProviderService = assetProviderService;
            _updateService = updateService;
            _vfxFactory = vfxFactory;
        }

        public void CreatePlayerGunShotProjectile(CreateProjectileDto createProjectileDto)
        {
            var projectile =
                _assetProviderService.Instantiate<Projectile>(AssetPaths.PlayerGunShotProjectilePrefab,
                    createProjectileDto.SpawnPosition + _playerProjectileOffset,
                    Quaternion.LookRotation(createProjectileDto.ForwardDirection));
            projectile.Init(createProjectileDto, _updateService, _vfxFactory, ProjectileType.Gunshot);
        }

        public void CreatePlayerBulletHellProjectile(CreateProjectileDto createProjectileDto)
        {
            var projectile =
                _assetProviderService.Instantiate<Projectile>(AssetPaths.PlayerBulletHellProjectilePrefab,
                    createProjectileDto.SpawnPosition + _playerProjectileOffset,
                    Quaternion.LookRotation(createProjectileDto.ForwardDirection));
            projectile.Init(createProjectileDto, _updateService, _vfxFactory, ProjectileType.BulletHell);
        }

        public void CreateEnemyProjectile(CreateEnemyProjectileDto createProjectileDto)
        {
            var projectile =
                _assetProviderService.Instantiate<Projectile>(AssetPaths.EnemyProjectilePrefab,
                    createProjectileDto.SpawnPosition + _enemyProjectileOffset,
                    Quaternion.LookRotation(createProjectileDto.ForwardDirection));
            projectile.Init(createProjectileDto, _updateService, _vfxFactory, ProjectileType.Enemy);
        }

        public void CreatePlayerBuzzSawProjectile(CreateProjectileDto createProjectileDto)
        {
            var projectile =
                _assetProviderService.Instantiate<BuzzSawProjectile>(AssetPaths.PlayerBuzzSawProjectilePrefab,
                    createProjectileDto.SpawnPosition + _playerProjectileOffset);
            projectile.Init(createProjectileDto, _updateService, true);
        }

        public void CreatePlayerPuddleProjectile(CreateProjectileDto createProjectileDto)
        {
            var projectile =
                _assetProviderService.Instantiate<PuddleProjectile>(AssetPaths.PlayerPuddleProjectilePrefab,
                    createProjectileDto.SpawnPosition);
            projectile.Init(createProjectileDto, _updateService);
        }

        public void CreatePlayerEnergyLineProjectile(CreateProjectileDto createProjectileDto)
        {
            var projectile =
                _assetProviderService.Instantiate<EnergyLineProjectile>(AssetPaths.PlayerEnergyLineProjectilePrefab,
                    createProjectileDto.SpawnPosition + _playerProjectileOffset);
            projectile.Init(createProjectileDto, _updateService);
        }
    }
}