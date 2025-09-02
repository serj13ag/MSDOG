using Core.Models.Data;
using Gameplay.Abilities.Core;
using Gameplay.Interfaces;

namespace Gameplay.Factories
{
    public interface IAbilityFactory
    {
        IAbility CreateAbility(AbilityData abilityData, IEntityWithAbilities entityWithAbilities);
    }
}