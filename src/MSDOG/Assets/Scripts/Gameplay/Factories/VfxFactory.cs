using Gameplay.Projectiles;
using UnityEngine;
using Utility.Pools;
using Object = UnityEngine.Object;

namespace Gameplay.Factories
{
    public class VfxFactory : IVfxFactory
    {
        private readonly GameObjectPoolRegistry<ProjectileImpactVFX> _pools = new();

        public void CreatProjectileImpactEffect(Vector3 position, ProjectileImpactVFX impactVFXPrefab)
        {
            var vfx = _pools.Get(impactVFXPrefab,
                () => Object.Instantiate(impactVFXPrefab, position, Quaternion.Euler(90f, 0f, 0f)));
            vfx.transform.position = position;
            vfx.Play();
        }
    }
}