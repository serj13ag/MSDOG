namespace Core.Abilities
{
    public abstract class BaseCooldownAbility : IAbility
    {
        private readonly float _cooldown;
        private float _timeTillAction;

        protected BaseCooldownAbility(float cooldown, float firstCooldownReduction)
        {
            _cooldown = cooldown;

            _timeTillAction = cooldown - firstCooldownReduction;
        }

        public void Activate()
        {
        }

        public void OnUpdate(float deltaTime)
        {
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
        }

        protected abstract void InvokeAction();

        private void ResetTimeTillAction()
        {
            _timeTillAction = _cooldown;
        }
    }
}