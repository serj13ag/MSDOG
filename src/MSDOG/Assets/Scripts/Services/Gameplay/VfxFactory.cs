using System;
using Constants;
using Core;
using UnityEngine;
using UtilityComponents;

namespace Services.Gameplay
{
    public class VfxFactory
    {
        private readonly Vector3 _playerAbilityVfxOffset = Vector3.up * 1f;

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
                position + _playerAbilityVfxOffset, Quaternion.Euler(90f, 0f, 0f));

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
                    _assetProviderService.Instantiate<SpriteAnimatorComponent>(AssetPaths.GunshotProjectileImpactVFXPath, position,
                        Quaternion.Euler(90f, 0f, 0f));
                    break;
                case ProjectileType.BulletHell:
                    _assetProviderService.Instantiate<SpriteAnimatorComponent>(AssetPaths.BulletHellProjectileImpactVFXPath, position,
                        Quaternion.Euler(90f, 0f, 0f));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(projectileType), projectileType, null);
            }
        }
    }
}