using UnityEngine;

namespace Gameplay.Interfaces
{
    public interface IMovingEntity : IEntityWithPosition, IEntityWithMoveSpeed
    {
        float RotationSpeed { get; }

        void SetRotation(Quaternion rotation);
        Quaternion GetRotation();
    }
}