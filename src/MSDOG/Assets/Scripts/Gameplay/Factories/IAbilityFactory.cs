using Core.Models.Data;
using Gameplay.Abilities.Core;

namespace Gameplay.Factories
{
    public interface IAbilityFactory
    {
        IAbility CreateAbility(AbilityData abilityData, Player player);
    }
}