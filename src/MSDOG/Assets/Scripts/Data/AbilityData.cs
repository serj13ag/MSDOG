using Core.Abilities;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "AbilityData", menuName = "Data/AbilityData")]
    public class AbilityData : ScriptableObject
    {
        public AbilityType AbilityType;
        public int Level;
        public int Damage;
        public float Cooldown;
        public float FirstCooldownReduction;
        public float Speed;
        public int Pierce;
        public float Size;
        public int DamageReductionPercent;
        public float Lifetime;
        public float TickTimeout;
    }
}