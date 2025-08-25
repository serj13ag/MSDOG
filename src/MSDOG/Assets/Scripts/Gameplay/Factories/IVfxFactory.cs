using Gameplay.Projectiles;
using UnityEngine;

namespace Gameplay.Factories
{
    public interface IVfxFactory
    {
        void CreatProjectileImpactEffect(Vector3 position, ProjectileImpactVFX impactVFXPrefab);
    }
}