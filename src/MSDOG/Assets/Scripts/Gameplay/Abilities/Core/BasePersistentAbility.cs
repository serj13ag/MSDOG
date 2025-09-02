using System;

namespace Gameplay.Abilities.Core
{
    public abstract class BasePersistentAbility : IAbility
    {
        private bool _isActive;

        public event Action OnActivated;
        public event Action OnDeactivated;
        public event Action OnDisposed;

        public void Activate()
        {
            if (_isActive)
            {
                return;
            }

            _isActive = true;

            Activated();
            OnActivated?.Invoke();
        }

        public void OnUpdate(float deltaTime)
        {
        }

        public void Deactivate()
        {
            if (!_isActive)
            {
                return;
            }

            _isActive = false;

            Deactivated();
            OnDeactivated?.Invoke();
        }

        protected abstract void Activated();
        protected abstract void Deactivated();

        public void Dispose()
        {
            OnDisposed?.Invoke();
        }
    }
}