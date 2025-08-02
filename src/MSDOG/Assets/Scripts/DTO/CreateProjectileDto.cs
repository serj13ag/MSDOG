using Core;
using Data;
using UnityEngine;

namespace DTO
{
    public class CreateProjectileDto
    {
        public Vector3 SpawnPosition { get; }
        public Vector3 ForwardDirection { get; }
        public Player Player { get; }
        public int Damage { get; }
        public float Speed { get; }
        public int Pierce { get; }
        public float Size { get; }
        public float TickTimeout { get; }
        public float Lifetime { get; }

        public CreateProjectileDto(Vector3 spawnPosition, Vector3 forwardDirection, Player player, AbilityData abilityData)
            : this(spawnPosition, forwardDirection, player, abilityData.Damage, abilityData.Speed, abilityData.Pierce,
                abilityData.Size, abilityData.TickTimeout, abilityData.Lifetime)
        {
        }

        public CreateProjectileDto(Vector3 spawnPosition, Vector3 forwardDirection, Player player, int damage, float speed,
            int pierce, float size, float tickTimeout, float lifetime)
        {
            SpawnPosition = spawnPosition;
            ForwardDirection = forwardDirection;
            Player = player;
            Damage = damage;
            Speed = speed;
            Pierce = pierce;
            Size = size;
            TickTimeout = tickTimeout;
            Lifetime = lifetime;
        }
    }
}