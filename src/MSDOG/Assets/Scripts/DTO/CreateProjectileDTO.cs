using UnityEngine;

namespace DTO
{
    public struct CreateProjectileDTO
    {
        public Vector3 SpawnPosition { get; }
        public Vector3 ForwardDirection { get; }
        public int Damage { get; }
        public float Speed { get; }
        public int Pierce { get; }

        public CreateProjectileDTO(Vector3 spawnPosition, Vector3 forwardDirection, int damage, float speed, int pierce)
        {
            SpawnPosition = spawnPosition;
            ForwardDirection = forwardDirection;
            Damage = damage;
            Speed = speed;
            Pierce = pierce;
        }
    }
}