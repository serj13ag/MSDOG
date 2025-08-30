using Core.Models.Data;
using Gameplay.AbilityVFX;

namespace Gameplay.Factories
{
    public interface IAbilityVFXFactory
    {
        void Prewarm(int levelIndex);
        T CreateEffect<T>(Player player, AbilityData abilityData) where T : BaseAbilityVFX;
    }
}