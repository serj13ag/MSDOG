using Gameplay.Interfaces;
using UnityEngine;

namespace Gameplay.Controllers
{
    public interface ICameraController
    {
        Camera GameplayCamera { get; }

        void SetFollowTarget(IEntityWithPosition entityWithPosition);
    }
}