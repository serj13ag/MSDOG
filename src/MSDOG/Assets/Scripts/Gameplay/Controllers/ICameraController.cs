using UnityEngine;

namespace Gameplay.Controllers
{
    public interface ICameraController
    {
        Camera Camera { get; }

        void SetFollowTarget(Transform targetTransform);
    }
}