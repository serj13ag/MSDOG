using UnityEngine;

namespace Gameplay.Interfaces
{
    public interface IEntityWithPosition
    {
        Vector3 GetPosition();
        Vector3 GetForwardDirection();
    }
}