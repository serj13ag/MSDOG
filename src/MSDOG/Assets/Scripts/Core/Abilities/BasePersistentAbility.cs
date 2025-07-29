using Data;
using Services;

namespace Core.Abilities
{
    public abstract class BasePersistentAbility : IAbility
    {
        private readonly AbilityData _abilityData;
        private readonly SoundService _soundService;

        private bool _isActive;

        protected BasePersistentAbility(AbilityData abilityData, SoundService soundService)
        {
            _abilityData = abilityData;
            _soundService = soundService;
        }

        public void Activate()
        {
            if (_isActive)
            {
                return;
            }

            _isActive = true;

            _soundService.PlayAbilityActivationSfx(_abilityData.ActivationSound);

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