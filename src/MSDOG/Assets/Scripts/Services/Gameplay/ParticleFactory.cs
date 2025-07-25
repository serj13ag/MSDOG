using Constants;
using UnityEngine;

namespace Services.Gameplay
{
    public class ParticleFactory
    {
        private const float HeightOffset = 1f;

        private readonly AssetProviderService _assetProviderService;

        public ParticleFactory(AssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
        }

        public void CreateBloodVfx(Vector3 position)
        {
            _assetProviderService.Instantiate<ParticleSystem>(AssetPaths.BloodParticlesVFXPath,
                position + Vector3.up * HeightOffset);
        }
    }
}