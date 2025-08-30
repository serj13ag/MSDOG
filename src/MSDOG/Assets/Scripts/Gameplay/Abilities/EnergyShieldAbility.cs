using Core.Controllers;
using Core.Models.Data;
using Gameplay.AbilityVFX;
using Gameplay.Factories;

namespace Gameplay.Abilities
{
    public class EnergyShieldAbility : BasePersistentAbility
    {
        private readonly IAbilityVFXFactory _abilityVFXFactory;

        private readonly AbilityData _abilityData;
        private readonly Player _player;
        private readonly int _damageReductionPercent;

        private FollowingAbilityVFX _followingAbilityVFX;

        public EnergyShieldAbility(AbilityData abilityData, Player player, IAbilityVFXFactory abilityVFXFactory,
            ISoundController soundController)
            : base(abilityData, soundController)
        {
            _abilityVFXFactory = abilityVFXFactory;

            _abilityData = abilityData;
            _player = player;
            _damageReductionPercent = abilityData.DamageReductionPercent;
        }

        protected override void OnActivated()
        {
            _player.ChangeDamageReductionPercent(_damageReductionPercent);

            _followingAbilityVFX = _abilityVFXFactory.CreateEffect<FollowingAbilityVFX>(_player, _abilityData);
        }

        protected override void OnDeactivated()
        {
            _player.ChangeDamageReductionPercent(-_damageReductionPercent);

            _followingAbilityVFX.Clear();
            _followingAbilityVFX = null;
        }
    }
}