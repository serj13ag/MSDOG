using System;
using Core.Models.Data;
using Gameplay.Abilities.Core;
using UnityEngine;

namespace Gameplay.Interfaces
{
    public interface IEntityWithAbilities : IEntityWithPosition, IEntityWithDamageReduction, IEntityWithAdditionalMoveSpeed
    {
        void AddAbility(Guid detailId, AbilityData detailAbilityData);
        void RemoveAbility(Guid detailId);

        void AbilitiesSetActive(bool value);

        Vector3 GetAbilitySpawnPosition(AbilityType abilityDataAbilityType);
    }
}