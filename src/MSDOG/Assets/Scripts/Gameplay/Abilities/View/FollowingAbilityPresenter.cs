using Core.Controllers;
using Core.Models.Data;
using Gameplay.Abilities.Core;
using Gameplay.Abilities.View.VFX;
using Gameplay.Factories;
using UnityEngine.Assertions;

namespace Gameplay.Abilities.View
{
    public class FollowingAbilityPresenter
    {
        private readonly IAbilityVFXFactory _abilityVFXFactory;
        private readonly ISoundController _soundController;

        private readonly Player _player;
        private readonly IAbility _ability;
        private readonly AbilityData _abilityData;

        private FollowingAbilityVFX _vfx;

        public FollowingAbilityPresenter(Player player, IAbility ability, AbilityData abilityData,
            IAbilityVFXFactory abilityVFXFactory, ISoundController soundController)
        {
            Assert.IsNotNull(abilityData.FollowingAbilityVFXPrefab);

            _abilityVFXFactory = abilityVFXFactory;
            _soundController = soundController;

            _player = player;
            _ability = ability;
            _abilityData = abilityData;

            ability.OnActivated += OnAbilityActivated;
            ability.OnDeactivated += OnAbilityDeactivated;
            ability.OnDisposed += OnAbilityDisposed;
        }

        private void OnAbilityActivated()
        {
            _vfx = _abilityVFXFactory.CreateEffect<FollowingAbilityVFX>(_player, _abilityData);
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