using Constants;
using Core.Services;
using Gameplay.VFX;
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

        public void CreateSlashEffect(Vector3 position, float length)
        {
            var t = Mathf.InverseLerp(7f, 9f, length);
            var scale = Mathf.LerpUnclamped(0.7f, 1f, t);

            var effect = _assetProviderService.Instantiate<Component>(AssetPaths.CuttingBlowVFXPath, position,
                Quaternion.Euler(90f, 0f, 0f));

            const float width = 1.4f;
            effect.transform.localScale = new Vector3(scale, width, 1f);
        }

        public void CreateRoundAttackEffect(Vector3 position, float radius)
        {
            var t = Mathf.InverseLerp(3f, 6f, radius);
            var scale = Mathf.LerpUnclamped(1f, 2.2f, t);

            var effect = _assetProviderService.Instantiate<Component>(AssetPaths.RoundAttackVFXPath, position,
                Quaternion.Euler(90f, 0f, 0f));
            effect.transform.localScale = new Vector3(scale, scale, scale);
        }

        public void CreatProjectileImpactEffect(Vector3 position, GameObject impactVFXPrefab)
        {
            Object.Instantiate(impactVFXPrefab, position, Quaternion.Euler(90f, 0f, 0f));
        }

        public FollowingAbilityEffect CreateAntiGravityEffect(Player player)
        {
            var effect = _assetProviderService.Instantiate<FollowingAbilityEffect>(AssetPaths.AntiGravityFollowingVFXPath);
            effect.Init(player);
            return effect;
        }

        public FollowingAbilityEffect CreateEnergyShieldEffect(Player player)
        {
            var effect = _assetProviderService.Instantiate<FollowingAbilityEffect>(AssetPaths.EnergyShieldFollowingVFXPath);
            effect.Init(player);
            return effect;
        }
    }
}