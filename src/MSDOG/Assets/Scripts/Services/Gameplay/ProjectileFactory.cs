using Constants;
using Core;
using DTO;
using UnityEngine;

namespace Services.Gameplay
{
    public class ProjectileFactory
    {
        private readonly AssetProviderService _assetProviderService;
        private readonly UpdateService _updateService;

        public ProjectileFactory(AssetProviderService assetProviderService, UpdateService updateService)
        {
            _assetProviderService = assetProviderService;
            _updateService = updateService;
        }

        public void CreatePlayerProjectile(CreateProjectileDTO createProjectileDto)
        {
            var projectile =
                _assetProviderService.Instantiate<Projectile>(AssetPaths.PlayerProjectilePrefab,
                    createProjectileDto.SpawnPosition);
            projectile.Init(createProjectileDto, _updateService);
        }
    }
}