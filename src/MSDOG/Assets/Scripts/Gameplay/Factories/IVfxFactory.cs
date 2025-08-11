using UnityEngine;

namespace Gameplay.Factories
{
    public interface IVfxFactory
    {
        void CreateBloodEffect(Vector3 position);
        void CreateSlashEffect(Vector3 position, float length);
        void CreateRoundAttackEffect(Vector3 position, float radius);
        void CreatProjectileImpactEffect(Vector3 position, GameObject impactVFXPrefab);
    }
}