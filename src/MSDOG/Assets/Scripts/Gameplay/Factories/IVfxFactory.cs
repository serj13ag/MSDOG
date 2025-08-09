using Gameplay.VFX;
using UnityEngine;

namespace Gameplay.Factories
{
    public interface IVfxFactory
    {
        void CreateBloodEffect(Vector3 position);
        void CreateSlashEffect(Vector3 position, float length);
        void CreateRoundAttackEffect(Vector3 position, float radius);
        void CreatProjectileImpactEffect(Vector3 position, GameObject impactVFXPrefab);

        FollowingAbilityEffect CreateAntiGravityEffect(Player player);
        FollowingAbilityEffect CreateEnergyShieldEffect(Player player);

        void CreateDamageTextEffect(int damageDealt, Vector3 position);
    }
}