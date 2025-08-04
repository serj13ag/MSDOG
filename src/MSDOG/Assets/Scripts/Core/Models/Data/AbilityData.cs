using Core.Sounds;
using Gameplay.Abilities;
using UnityEngine;

namespace Core.Models.Data
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
        public int Pierce = -1;
        public float Size;
        public int DamageReductionPercent;
        public float Lifetime;
        public float TickTimeout;

        public Sprite Icon;
        public SoundClip ActivationSound;
        public ProjectileData ProjectileData;
    }
}