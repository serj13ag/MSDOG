using UnityEngine;

namespace Gameplay.Factories
{
    public interface IVfxFactory
    {
        void CreateBloodEffect(Vector3 position);
        void CreatProjectileImpactEffect(Vector3 position, GameObject impactVFXPrefab);
    }
}