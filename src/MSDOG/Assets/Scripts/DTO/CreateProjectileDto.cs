using Core;
using UnityEngine;

namespace DTO
{
    public struct CreateProjectileDto
    {
        public Vector3 SpawnPosition { get; }
        public Vector3 ForwardDirection { get; }
        public Player Player { get; }
        public int Damage { get; }
        public float Speed { get; }
        public int Pierce { get; }

        public CreateProjectileDto(Vector3 spawnPosition, Vector3 forwardDirection, Player player, int damage, float speed,
            int pierce)
        {
            SpawnPosition = spawnPosition;
            ForwardDirection = forwardDirection;
            Player = player;
            Damage = damage;
            Speed = speed;
            Pierce = pierce;
        }
    }
}