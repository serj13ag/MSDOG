namespace Gameplay.Interfaces
{
    public interface IMovingEntity : IEntityWithPosition, IEntityWithRotation, IEntityWithMoveSpeed
    {
        float RotationSpeed { get; }
    }
}