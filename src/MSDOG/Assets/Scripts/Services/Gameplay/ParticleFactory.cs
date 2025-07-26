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

        public void CreateSlashEffect(Vector3 position, float length)
        {
            var t = Mathf.InverseLerp(7f, 9f, length);
            var scale = Mathf.LerpUnclamped(0.7f, 1f, t);

            var effect = _assetProviderService.Instantiate<Component>(AssetPaths.CuttingBlowVFXPath,
                position + Vector3.up * 2f, Quaternion.Euler(90f, 0f, 0f));
            effect.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}