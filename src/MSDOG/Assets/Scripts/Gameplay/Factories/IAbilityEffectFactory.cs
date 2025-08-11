namespace Gameplay.Factories
{
    public interface IAbilityEffectFactory
    {
        void Prewarm(int levelIndex);
        FollowingAbilityEffect CreateFollowingEffect(FollowingAbilityEffect abilityEffectPrefab, Player player);
    }
}