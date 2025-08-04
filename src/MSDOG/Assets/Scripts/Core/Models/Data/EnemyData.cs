using Gameplay.Enemies;
using UnityEngine;

namespace Core.Models.Data
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public EnemyType Type;
        public float Speed;
        public int MaxHealth;
        public float Cooldown;
        public int Damage;
        public float ProjectileSpeed;
        public int Experience = 1;
        public Enemy Prefab;
        public EnemyDeathkit DeathkitPrefab;
    }
}