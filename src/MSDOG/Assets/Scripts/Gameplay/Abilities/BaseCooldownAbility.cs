using Core.Controllers;
using Core.Models.Data;

namespace Gameplay.Abilities
{
    public abstract class BaseCooldownAbility : IAbility
    {
        private readonly AbilityData _abilityData;
        private readonly ISoundController _soundController;

        private readonly float _cooldown;

        private bool _isActive;
        private float _timeTillAction;

        protected AbilityType AbilityType => _abilityData.AbilityType;

        protected BaseCooldownAbility(AbilityData abilityData, ISoundController soundController)
        {
            _cooldown = abilityData.Cooldown;
            _abilityData = abilityData;
            _soundController = soundController;

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

            _soundController.PlayAbilityActivationSfx(_abilityData.ActivationSound);
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