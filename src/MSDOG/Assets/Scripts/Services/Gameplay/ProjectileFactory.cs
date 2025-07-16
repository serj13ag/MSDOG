using Constants;
using Core;
using DTO;

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

        public void CreatePlayerProjectile(CreateProjectileDto createProjectileDto)
        {
            var projectile =
                _assetProviderService.Instantiate<Projectile>(AssetPaths.PlayerProjectilePrefab,
                    createProjectileDto.SpawnPosition);
            projectile.Init(createProjectileDto, _updateService, true);
        }

        public void CreateEnemyProjectile(CreateProjectileDto createProjectileDto)
        {
            var projectile =
                _assetProviderService.Instantiate<Projectile>(AssetPaths.EnemyProjectilePrefab,
                    createProjectileDto.SpawnPosition);
            projectile.Init(createProjectileDto, _updateService, false);
        }
    }
}