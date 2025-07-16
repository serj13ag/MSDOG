using Core.Enemies;
using UnityEngine;

namespace Data
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
    }
}