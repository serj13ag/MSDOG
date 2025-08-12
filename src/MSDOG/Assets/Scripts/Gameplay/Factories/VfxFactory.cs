using Constants;
using Core.Services;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.Factories
{
    public class VfxFactory : IVfxFactory
    {
        private readonly IAssetProviderService _assetProviderService;

        public VfxFactory(IAssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
        }

        public void CreateBloodEffect(Vector3 position)
        {
            _assetProviderService.Instantiate<ParticleSystem>(AssetPaths.BloodParticlesVFXPath, position);
        }

        public void CreatProjectileImpactEffect(Vector3 position, GameObject impactVFXPrefab)
        {
            Object.Instantiate(impactVFXPrefab, position, Quaternion.Euler(90f, 0f, 0f));
        }
    }
}