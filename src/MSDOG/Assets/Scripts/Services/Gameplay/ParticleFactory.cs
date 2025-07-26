using Constants;
using UnityEngine;

namespace Services.Gameplay
{
    public class ParticleFactory
    {
        private readonly Vector3 _playerAbilityVfxOffset = Vector3.up * 1f;

        private readonly AssetProviderService _assetProviderService;

        public ParticleFactory(AssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
        }

        public void CreateBloodVfx(Vector3 position)
        {
            _assetProviderService.Instantiate<ParticleSystem>(AssetPaths.BloodParticlesVFXPath,
                position + _playerAbilityVfxOffset);
        }

        public void CreateSlashEffect(Vector3 position, float length)
        {
            var t = Mathf.InverseLerp(7f, 9f, length);
            var scale = Mathf.LerpUnclamped(0.7f, 1f, t);

            var effect = _assetProviderService.Instantiate<Component>(AssetPaths.CuttingBlowVFXPath,
                position + _playerAbilityVfxOffset, Quaternion.Euler(90f, 0f, 0f));
            effect.transform.localScale = new Vector3(scale, scale, scale);
        }

        public void CreateRoundAttackEffect(Vector3 position, float radius)
        {
            var t = Mathf.InverseLerp(3f, 6f, radius);
            var scale = Mathf.LerpUnclamped(1f, 2.2f, t);

            var effect = _assetProviderService.Instantiate<Component>(AssetPaths.RoundAttackVFXPath,
                position + _playerAbilityVfxOffset, Quaternion.Euler(90f, 0f, 0f));
            effect.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}