using Gameplay.Abilities.Core;
using UnityEngine;

namespace Gameplay.Interfaces
{
    public interface IEntityWithAbilities : IEntityWithPosition, IEntityWithDamageReduction, IEntityWithAdditionalMoveSpeed
    {
        Vector3 GetAbilitySpawnPosition(AbilityType abilityDataAbilityType);
    }
}