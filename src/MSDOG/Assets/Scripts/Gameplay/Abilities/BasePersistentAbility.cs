using Core.Controllers;
using Core.Models.Data;

namespace Gameplay.Abilities
{
    public abstract class BasePersistentAbility : IAbility
    {
        private readonly AbilityData _abilityData;
        private readonly SoundController _soundController;

        private bool _isActive;

        protected BasePersistentAbility(AbilityData abilityData, SoundController soundController)
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

            _soundController.PlayAbilityActivationSfx(_abilityData.ActivationSound);

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