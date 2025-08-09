using UnityEngine;

namespace Gameplay.Controllers
{
    public interface ICameraController
    {
        void SetFollowTarget(Transform targetTransform);
    }
}