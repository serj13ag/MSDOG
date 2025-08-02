using Core.Projectiles;
using Core.Projectiles.Views;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "Data/ProjectileData")]
    public class ProjectileData : ScriptableObject
    {
        public ProjectileType Type;
        public BaseProjectileView ViewPrefab;
        public GameObject ImpactVFXPrefab;
    }
}