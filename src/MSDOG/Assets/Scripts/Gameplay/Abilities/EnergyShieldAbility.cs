using Core.Controllers;
using Core.Models.Data;
using Gameplay.Factories;
using Gameplay.VFX;

namespace Gameplay.Abilities
{
    public class EnergyShieldAbility : BasePersistentAbility
    {
        private readonly IVfxFactory _vfxFactory;
        private readonly Player _player;
        private readonly int _damageReductionPercent;

        private FollowingAbilityEffect _followingAbilityEffect;

        public EnergyShieldAbility(AbilityData abilityData, Player player, IVfxFactory vfxFactory, ISoundController soundController)
            : base(abilityData, soundController)
        {
            _vfxFactory = vfxFactory;
            _player = player;
            _damageReductionPercent = abilityData.DamageReductionPercent;
        }

        protected override void OnActivated()
        {
            _player.ChangeDamageReductionPercent(_damageReductionPercent);

            _followingAbilityEffect = _vfxFactory.CreateEnergyShieldEffect(_player);
        }

        protected override void OnDeactivated()
        {
            _player.ChangeDamageReductionPercent(-_damageReductionPercent);

            _followingAbilityEffect.Clear();
            _followingAbilityEffect = null;
        }
    }
}