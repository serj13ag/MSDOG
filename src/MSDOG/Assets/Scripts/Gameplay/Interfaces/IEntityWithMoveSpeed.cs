namespace Gameplay.Interfaces
{
    public interface IEntityWithMoveSpeed
    {
        float BaseMoveSpeed { get; }
        float CurrentMoveSpeed { get; }
    }
}