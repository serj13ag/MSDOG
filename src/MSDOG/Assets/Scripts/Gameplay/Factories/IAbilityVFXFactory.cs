using Core.Models.Data;
using Gameplay.Abilities.View.VFX;
using Gameplay.Interfaces;

namespace Gameplay.Factories
{
    public interface IAbilityVFXFactory
    {
        void Prewarm(int levelIndex);
        T CreateEffect<T>(IEntityWithAbilities entityWithAbilities, AbilityData abilityData) where T : BaseAbilityVFX;
    }
}