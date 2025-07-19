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
        public AbilityData AbilityData { get; }

        public CreateProjectileDto(Vector3 spawnPosition, Vector3 forwardDirection, Player player, AbilityData abilityData)
        {
            SpawnPosition = spawnPosition;
            ForwardDirection = forwardDirection;
            Player = player;
            AbilityData = abilityData;
        }
    }
}