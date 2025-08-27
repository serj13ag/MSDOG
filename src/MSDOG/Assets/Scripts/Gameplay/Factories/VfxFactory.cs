using Gameplay.Projectiles;
using Gameplay.Providers;
using UnityEngine;
using Utility.Pools;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Factories
{
    public class VfxFactory : IVfxFactory
    {
        private readonly IObjectResolver _container;
        private readonly IObjectContainerProvider _objectContainerProvider;

        private readonly GameObjectPoolRegistry<ProjectileImpactVFX> _pools = new();

        public VfxFactory(IObjectResolver container, IObjectContainerProvider objectContainerProvider)
        {
            _container = container;
            _objectContainerProvider = objectContainerProvider;
        }

        public void CreatProjectileImpactEffect(Vector3 position, ProjectileImpactVFX impactVFXPrefab)
        {
            var vfx = _pools.Get(impactVFXPrefab,
                () => _container.Instantiate(impactVFXPrefab, position, Quaternion.Euler(90f, 0f, 0f),
                    _objectContainerProvider.ProjectileVFXContainer));
            vfx.transform.position = position;
            vfx.Play();
        }
    }
}