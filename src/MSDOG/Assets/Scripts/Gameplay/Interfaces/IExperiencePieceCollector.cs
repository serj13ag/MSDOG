namespace Gameplay.Interfaces
{
    public interface IExperiencePieceCollector : IEntityWithPosition
    {
        void CollectExperience(int experience);
    }
}