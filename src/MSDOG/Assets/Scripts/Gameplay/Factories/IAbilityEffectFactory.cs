using Core.Models.Data;
using Gameplay.AbilityEffects;

namespace Gameplay.Factories
{
    public interface IAbilityEffectFactory
    {
        void Prewarm(int levelIndex);
        T CreateEffect<T>(Player player, AbilityData abilityData) where T : BaseAbilityEffect;
    }
}