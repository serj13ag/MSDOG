using Gameplay.Interfaces;

namespace Gameplay
{
    public interface IPlayer : IMovingEntity, IEntityWithHealth, IEntityWithAbilities, IExperiencePieceCollector,
        IProjectileDamageableEntity, IOverlapDamageableEntity, IEntityWithNitro
    {
        void Heal(int amount);
    }
}