using Core.Controllers;
using Core.Models.Data;
using Gameplay.Abilities.Core;
using Gameplay.Abilities.View.VFX;
using Gameplay.Factories;
using Gameplay.Interfaces;
using UnityEngine.Assertions;

namespace Gameplay.Abilities.View
{
    public class FollowingAbilityPresenter
    {
        private readonly IAbilityVFXFactory _abilityVFXFactory;
        private readonly ISoundController _soundController;

        private readonly IEntityWithAbilities _entityWithAbilities;
        private readonly IAbility _ability;
        private readonly AbilityData _abilityData;

        private FollowingAbilityVFX _vfx;

        public FollowingAbilityPresenter(IEntityWithAbilities entityWithAbilities, IAbility ability, AbilityData abilityData,
            IAbilityVFXFactory abilityVFXFactory, ISoundController soundController)
        {
            Assert.IsNotNull(abilityData.FollowingAbilityVFXPrefab);

            _abilityVFXFactory = abilityVFXFactory;
            _soundController = soundController;

            _entityWithAbilities = entityWithAbilities;
            _ability = ability;
            _abilityData = abilityData;

            ability.OnActivated += OnAbilityActivated;
            ability.OnDeactivated += OnAbilityDeactivated;
            ability.OnDisposed += OnAbilityDisposed;
        }

        private void OnAbilityActivated()
        {
            _vfx = _abilityVFXFactory.CreateEffect<FollowingAbilityVFX>(_entityWithAbilities, _abilityData);
            _soundController.PlayAbilityActivationSfx(_abilityData.ActivationSound);
        }

        private void OnAbilityDeactivated()
        {
            _vfx.Clear();
            _vfx = null;
        }

        private void OnAbilityDisposed()
        {
            _ability.OnActivated -= OnAbilityActivated;
            _ability.OnDeactivated -= OnAbilityDeactivated;
            _ability.OnDisposed -= OnAbilityDisposed;
        }
    }
}