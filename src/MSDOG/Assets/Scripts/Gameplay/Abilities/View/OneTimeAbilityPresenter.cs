using Core.Controllers;
using Core.Models.Data;
using Gameplay.Abilities.Core;
using Gameplay.Abilities.View.VFX;
using Gameplay.Factories;
using Gameplay.Interfaces;
using UnityEngine.Assertions;

namespace Gameplay.Abilities.View
{
    public class OneTimeAbilityPresenter
    {
        private readonly IAbilityVFXFactory _abilityVFXFactory;
        private readonly ISoundController _soundController;

        private readonly IEntityWithAbilities _entityWithAbilities;
        private readonly ICooldownAbility _ability;
        private readonly AbilityData _abilityData;

        public OneTimeAbilityPresenter(IEntityWithAbilities entityWithAbilities, ICooldownAbility ability,
            AbilityData abilityData, IAbilityVFXFactory abilityVFXFactory, ISoundController soundController)
        {
            Assert.IsNotNull(abilityData.FollowingAbilityVFXPrefab);

            _abilityVFXFactory = abilityVFXFactory;
            _soundController = soundController;

            _entityWithAbilities = entityWithAbilities;
            _ability = ability;
            _abilityData = abilityData;

            ability.OnActionInvoked += OnAbilityActionInvoked;
            ability.OnDisposed += OnAbilityDisposed;
        }

        private void OnAbilityActionInvoked()
        {
            _abilityVFXFactory.CreateEffect<OneTimeAbilityVFX>(_entityWithAbilities, _abilityData);
            _soundController.PlayAbilityActivationSfx(_abilityData.ActivationSound);
        }

        private void OnAbilityDisposed()
        {
            _ability.OnActionInvoked -= OnAbilityActionInvoked;
            _ability.OnDisposed -= OnAbilityDisposed;
        }
    }
}