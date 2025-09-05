using Gameplay.Projectiles;

namespace Gameplay.Factories
{
    public interface IProjectileFactory
    {
        void Prewarm(int levelIndex);

        void CreateProjectile(ProjectileSpawnData projectileSpawnData);
    }
}