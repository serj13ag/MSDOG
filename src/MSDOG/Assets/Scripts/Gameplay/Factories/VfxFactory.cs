using Core.Services;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.Factories
{
    public class VfxFactory : IVfxFactory
    {
        private readonly IAssetProviderService _assetProviderService;

        // TODO: add pool
        public VfxFactory(IAssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
        }

        public void CreateBloodEffect(Vector3 position)
        {
            // TODO: add
        }

        public void CreatProjectileImpactEffect(Vector3 position, GameObject impactVFXPrefab)
        {
            Object.Instantiate(impactVFXPrefab, position, Quaternion.Euler(90f, 0f, 0f));
        }
    }
}