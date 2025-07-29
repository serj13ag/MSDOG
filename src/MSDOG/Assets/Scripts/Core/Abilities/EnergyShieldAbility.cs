using Data;
using Services;
using Services.Gameplay;
using VFX;

namespace Core.Abilities
{
    public class EnergyShieldAbility : BasePersistentAbility
    {
        private readonly VfxFactory _vfxFactory;
        private readonly Player _player;
        private readonly int _damageReductionPercent;

        private FollowingAbilityEffect _followingAbilityEffect;

        public EnergyShieldAbility(AbilityData abilityData, Player player, VfxFactory vfxFactory, SoundService soundService)
            : base(abilityData, soundService)
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