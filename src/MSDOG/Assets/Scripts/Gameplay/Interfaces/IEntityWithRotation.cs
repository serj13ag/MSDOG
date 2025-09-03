using UnityEngine;

namespace Gameplay.Interfaces
{
    public interface IEntityWithRotation
    {
        Quaternion GetRotation();
        void SetRotation(Quaternion rotation);
    }
}