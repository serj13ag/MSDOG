namespace Gameplay.Interfaces
{
    public interface IExperiencePieceCollector : IEntityWithExperience, IEntityWithPosition
    {
        void CollectExperience(int experience);
    }
}