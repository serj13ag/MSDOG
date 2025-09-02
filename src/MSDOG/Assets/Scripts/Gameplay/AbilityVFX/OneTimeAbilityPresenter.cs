using Core.Models.Data;
using Gameplay.Abilities;
using Gameplay.Factories;
using UnityEngine.Assertions;

namespace Gameplay.AbilityVFX
{
    public class OneTimeAbilityPresenter
    {
        private readonly IAbilityVFXFactory _abilityVFXFactory;

        private readonly Player _player;
        private readonly IAbility _ability;
        private readonly AbilityData _abilityData;

        public OneTimeAbilityPresenter(Player player, IAbility ability, AbilityData abilityData,
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