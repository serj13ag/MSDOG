using Gameplay.Projectiles;

namespace Gameplay.Factories
{
    public interface IProjectileFactory
    {
        void Prewarm(int levelIndex);

        void CreateAbilityProjectile(ProjectileSpawnData projectileSpawnData);
        void CreateEnemyProjectile(ProjectileSpawnData projectileSpawnData);
    }
}