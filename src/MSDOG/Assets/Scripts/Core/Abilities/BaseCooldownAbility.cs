namespace Core.Abilities
{
    public abstract class BaseCooldownAbility : IAbility
    {
        private readonly float _cooldown;

        private bool _isActive;
        private float _timeTillAction;

        protected BaseCooldownAbility(float cooldown, float firstCooldownReduction)
        {
            _cooldown = cooldown;

            _timeTillAction = cooldown - firstCooldownReduction;
        }

        public void Activate()
        {
            if (_isActive)
            {
                return;
            }

            _isActive = true;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_isActive)
            {
                return;
            }

            if (_timeTillAction > 0f)
            {
                _timeTillAction -= deltaTime;
                return;
            }

            InvokeAction();
            ResetTimeTillAction();
        }

        public void Deactivate()
        {
            if (!_isActive)
            {
                return;
            }

            _isActive = false;
        }

        protected abstract void InvokeAction();

        private void ResetTimeTillAction()
        {
            _timeTillAction = _cooldown;
        }
    }
}