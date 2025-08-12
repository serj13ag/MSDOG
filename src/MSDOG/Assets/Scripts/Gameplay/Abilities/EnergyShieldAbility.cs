using Core.Controllers;
using Core.Models.Data;
using Gameplay.AbilityEffects;
using Gameplay.Factories;

namespace Gameplay.Abilities
{
    public class EnergyShieldAbility : BasePersistentAbility
    {
        private readonly IAbilityEffectFactory _abilityEffectFactory;

        private readonly AbilityData _abilityData;
        private readonly Player _player;
        private readonly int _damageReductionPercent;

        private FollowingAbilityEffect _followingAbilityEffect;

        public EnergyShieldAbility(AbilityData abilityData, Player player, IAbilityEffectFactory abilityEffectFactory,
            ISoundController soundController)
            : base(abilityData, soundController)
        {
            _abilityEffectFactory = abilityEffectFactory;

            _abilityData = abilityData;
            _player = player;
            _damageReductionPercent = abilityData.DamageReductionPercent;
        }

        protected override void OnActivated()
        {
            _player.ChangeDamageReductionPercent(_damageReductionPercent);

            _followingAbilityEffect = _abilityEffectFactory.CreateEffect<FollowingAbilityEffect>(_player, _abilityData);
        }

        protected override void OnDeactivated()
        {
            _player.ChangeDamageReductionPercent(-_damageReductionPercent);

            _followingAbilityEffect.Clear();
            _followingAbilityEffect = null;
        }
    }
}