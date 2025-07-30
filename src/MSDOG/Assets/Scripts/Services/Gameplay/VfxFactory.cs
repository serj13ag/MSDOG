using System;
using Constants;
using Core;
using UI;
using UnityEngine;
using UtilityComponents;
using VFX;

namespace Services.Gameplay
{
    public class VfxFactory
    {
        private readonly Vector3 _playerAbilityVfxOffset = Vector3.up * 1f;
        private readonly Vector3 _playerSlashEffectVfxOffset = Vector3.up * 1.55f;
        private readonly Vector3 _damageTextOffset = Vector3.up * 3f;

        private readonly AssetProviderService _assetProviderService;

        public VfxFactory(AssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
        }

        public void CreateBloodEffect(Vector3 position)
        {
            _assetProviderService.Instantiate<ParticleSystem>(AssetPaths.BloodParticlesVFXPath,
                position + _playerAbilityVfxOffset);
        }

        public void CreateSlashEffect(Vector3 position, float length)
        {
            var t = Mathf.InverseLerp(7f, 9f, length);
            var scale = Mathf.LerpUnclamped(0.7f, 1f, t);

            var effect = _assetProviderService.Instantiate<Component>(AssetPaths.CuttingBlowVFXPath,
                position + _playerSlashEffectVfxOffset, Quaternion.Euler(90f, 0f, 0f));

            const float width = 1.4f;
            effect.transform.localScale = new Vector3(scale, width, 1f);
        }

        public void CreateRoundAttackEffect(Vector3 position, float radius)
        {
            var t = Mathf.InverseLerp(3f, 6f, radius);
            var scale = Mathf.LerpUnclamped(1f, 2.2f, t);

            var effect = _assetProviderService.Instantiate<Component>(AssetPaths.RoundAttackVFXPath,
                position + _playerAbilityVfxOffset, Quaternion.Euler(90f, 0f, 0f));
            effect.transform.localScale = new Vector3(scale, scale, scale);
        }

        public void CreatEnemyProjectileImpactEffect(Vector3 position, ProjectileType projectileType)
        {
            switch (projectileType)
            {
                case ProjectileType.Enemy:
                    _assetProviderService.Instantiate<SpriteAnimatorComponent>(AssetPaths.EnemyProjectileImpactVFXPath, position,
                        Quaternion.Euler(90f, 0f, 0f));
                    break;
                case ProjectileType.Gunshot:
                    _assetProviderService.Instantiate<SpriteAnimatorComponent>(AssetPaths.GunshotProjectileImpactVFXPath,
                        position, Quaternion.Euler(90f, 0f, 0f));
                    break;
                case ProjectileType.BulletHell:
                    _assetProviderService.Instantiate<SpriteAnimatorComponent>(AssetPaths.BulletHellProjectileImpactVFXPath,
                        position, Quaternion.Euler(90f, 0f, 0f));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(projectileType), projectileType, null);
            }
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

        public void CreateDamageTextEffect(int damageDealt, Vector3 position)
        {
            var damageTextView = _assetProviderService.Instantiate<DamageTextView>(AssetPaths.DamageTextViewPrefabPath,
                position + _damageTextOffset, Quaternion.Euler(90f, 0f, 0f));
            damageTextView.Init(damageDealt);
        }
    }
}