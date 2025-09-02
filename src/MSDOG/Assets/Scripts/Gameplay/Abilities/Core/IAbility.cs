using System;

namespace Gameplay.Abilities.Core
{
    public interface IAbility : IDisposable
    {
        void Activate();
        void OnUpdate(float deltaTime);
        void Deactivate();

        event Action OnActivated;
        event Action OnDeactivated;
        event Action OnActionInvoked;
        event Action OnDisposed;
    }
}