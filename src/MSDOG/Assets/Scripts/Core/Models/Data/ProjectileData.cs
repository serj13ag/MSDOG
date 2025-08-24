using Gameplay.Projectiles;
using Gameplay.Projectiles.Views;
using UnityEngine;

namespace Core.Models.Data
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "Data/ProjectileData")]
    public class ProjectileData : ScriptableObject
    {
        public ProjectileType Type;
        public BaseProjectileView ViewPrefab;
        public ProjectileImpactVFX ImpactVFXPrefab;
    }
}