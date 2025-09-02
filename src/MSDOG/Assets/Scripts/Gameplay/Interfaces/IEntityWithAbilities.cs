using Gameplay.Abilities.Core;
using UnityEngine;

namespace Gameplay.Interfaces
{
    public interface IEntityWithAbilities : IEntityWithPosition // TODO: rename?
    {
        Vector3 GetAbilitySpawnPosition(AbilityType abilityDataAbilityType);
    }
}