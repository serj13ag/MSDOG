using Core.Models.Data;
using Gameplay.Abilities.View.VFX;

namespace Gameplay.Factories
{
    public interface IAbilityVFXFactory
    {
        void Prewarm(int levelIndex);
        T CreateEffect<T>(Player player, AbilityData abilityData) where T : BaseAbilityVFX;
    }
}