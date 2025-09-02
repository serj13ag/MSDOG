using System;
using Core.Controllers;
using Core.Models.Data;

namespace Gameplay.Abilities.Core
{
    public abstract class BasePersistentAbility : IAbility
    {
        private readonly AbilityData _abilityData;
        private readonly ISoundController _soundController;

        private bool _isActive;

        public event Action OnActivated;
        public event Action OnDeactivated;
        public event Action OnDisposed;

        protected BasePersistentAbility(AbilityData abilityData, ISoundController soundController)
        {
            _abilityData = abilityData;
            _soundController = soundController;
        }

        public void Activate()
        {
            if (_isActive)
            {
                return;
            }

            _isActive = true;

            _soundController.PlayAbilityActivationSfx(_abilityData.ActivationSound); // TODO: to presenter

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