using System;

namespace Core.Abilities
{
    public abstract class BaseCooldownAbility : IAbility
    {
        private readonly float _cooldown;
        private float _timeTillAction;

        public Guid Id { get; private set; }

        protected BaseCooldownAbility(float cooldown)
        {
            _cooldown = cooldown;

            ResetTimeTillAction();
        }

        public void SetId(Guid id)
        {
            Id = id;
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

        protected abstract void InvokeAction();

        private void ResetTimeTillAction()
        {
            _timeTillAction = _cooldown;
        }
    }
}