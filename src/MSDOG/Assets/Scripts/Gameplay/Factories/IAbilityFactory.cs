using Core.Models.Data;
using Gameplay.Abilities;

namespace Gameplay.Factories
{
    public interface IAbilityFactory
    {
        IAbility CreateAbility(AbilityData abilityData, Player player);
    }
}