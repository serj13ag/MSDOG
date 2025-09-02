namespace Gameplay.Interfaces
{
    public interface IEntityWithAdditionalMoveSpeed : IEntityWithMoveSpeed
    {
        void ChangeAdditionalSpeed(float speed);
    }
}