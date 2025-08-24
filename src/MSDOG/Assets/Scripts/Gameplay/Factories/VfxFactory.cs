using Gameplay.Projectiles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.Factories
{
    public class VfxFactory : IVfxFactory
    {
        // TODO: add pool
        public void CreateBloodEffect(Vector3 position)
        {
            // TODO: add
        }

        public void CreatProjectileImpactEffect(Vector3 position, ProjectileImpactVFX impactVFXPrefab)
        {
            var vfx = Object.Instantiate(impactVFXPrefab, position, Quaternion.Euler(90f, 0f, 0f));
            vfx.Play();
        }
    }
}