using Core.Models.Data;
using Gameplay.Abilities.Core;
using Gameplay.Abilities.View.VFX;
using Gameplay.Factories;
using UnityEngine.Assertions;

namespace Gameplay.Abilities.View
{
    public class OneTimeAbilityPresenter
    {
        private readonly IAbilityVFXFactory _abilityVFXFactory;

        private readonly Player _player;
        private readonly ICooldownAbility _ability;
        private readonly AbilityData _abilityData;

        public OneTimeAbilityPresenter(Player player, ICooldownAbility ability, AbilityData abilityData,
            IAbilityVFXFactory abilityVFXFactory)
        {
            Assert.IsNotNull(abilityData.FollowingAbilityVFXPrefab);

            _abilityVFXFactory = abilityVFXFactory;

            _player = player;
            _ability = ability;
            _abilityData = abilityData;

            ability.OnActionInvoked += OnAbilityActionInvoked;
            ability.OnDisposed += OnAbilityDisposed;
        }

        private void OnAbilityActionInvoked()
        {
            _abilityVFXFactory.CreateEffect<OneTimeAbilityVFX>(_player, _abilityData);
        }

        private void OnAbilityDisposed()
        {
            _ability.OnActionInvoked -= OnAbilityActionInvoked;
            _ability.OnDisposed -= OnAbilityDisposed;
        }
    }
}