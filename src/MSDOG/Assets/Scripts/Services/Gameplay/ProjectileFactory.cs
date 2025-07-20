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

        public void CreateEnemyProjectile(CreateEnemyProjectileDto createProjectileDto)
        {
            var projectile =
                _assetProviderService.Instantiate<Projectile>(AssetPaths.EnemyProjectilePrefab,
                    createProjectileDto.SpawnPosition);
            projectile.Init(createProjectileDto, _updateService, false);
        }

        public void CreatePlayerBuzzSawProjectile(CreateProjectileDto createProjectileDto)
        {
            var projectile =
                _assetProviderService.Instantiate<BuzzSawProjectile>(AssetPaths.PlayerBuzzSawProjectilePrefab,
                    createProjectileDto.SpawnPosition);
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
                    createProjectileDto.SpawnPosition);
            projectile.Init(createProjectileDto, _updateService);
        }
    }
}