using System;
using Core.Models.Data;

namespace Gameplay.Abilities.Core
{
    public abstract class BaseCooldownAbility : ICooldownAbility
    {
        private readonly float _cooldown;

        private bool _isActive;
        private float _timeTillAction;

        public event Action OnActivated;
        public event Action OnDeactivated;
        public event Action OnActionInvoked;
        public event Action OnDisposed;

        protected BaseCooldownAbility(AbilityData abilityData)
        {
            _cooldown = abilityData.Cooldown;
            _timeTillAction = abilityData.Cooldown - abilityData.FirstCooldownReduction;
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


            OnActionInvoked?.Invoke();
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

        public void Dispose()
        {
            OnDisposed?.Invoke();
        }
    }
}