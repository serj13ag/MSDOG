namespace Core.Abilities
{
    public abstract class BasePersistentAbility : IAbility
    {
        private bool _isActive;

        public void Activate()
        {
            if (_isActive)
            {
                return;
            }

            _isActive = true;

            OnActivated();
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

            OnDeactivated();
        }

        protected abstract void OnActivated();
        protected abstract void OnDeactivated();
    }
}