using Core.Controllers;
using Core.Models.Data;
using Gameplay.Factories;

namespace Gameplay.Abilities
{
    public class EnergyShieldAbility : BasePersistentAbility
    {
        private readonly IAbilityEffectFactory _abilityEffectFactory;

        private readonly Player _player;
        private readonly int _damageReductionPercent;
        private readonly FollowingAbilityEffect _followingAbilityEffectPrefab;

        private FollowingAbilityEffect _followingAbilityEffect;

        public EnergyShieldAbility(AbilityData abilityData, Player player, IAbilityEffectFactory abilityEffectFactory,
            ISoundController soundController)
            : base(abilityData, soundController)
        {
            _abilityEffectFactory = abilityEffectFactory;

            _player = player;
            _damageReductionPercent = abilityData.DamageReductionPercent;
            _followingAbilityEffectPrefab = abilityData.FollowingAbilityEffectPrefab;
        }

        protected override void OnActivated()
        {
            _player.ChangeDamageReductionPercent(_damageReductionPercent);

            _followingAbilityEffect = _abilityEffectFactory.CreateFollowingEffect(_followingAbilityEffectPrefab, _player);
        }

        protected override void OnDeactivated()
        {
            _player.ChangeDamageReductionPercent(-_damageReductionPercent);

            _followingAbilityEffect.Clear();
            _followingAbilityEffect = null;
        }
    }
}