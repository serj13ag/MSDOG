using Data;
using Services.Gameplay;
using VFX;

namespace Core.Abilities
{
    public class EnergyShieldAbility : IAbility
    {
        private readonly VfxFactory _vfxFactory;
        private readonly Player _player;
        private readonly int _damageReductionPercent;

        private FollowingAbilityEffect _followingAbilityEffect;

        public EnergyShieldAbility(AbilityData abilityData, Player player, VfxFactory vfxFactory)
        {
            _vfxFactory = vfxFactory;
            _player = player;
            _damageReductionPercent = abilityData.DamageReductionPercent;
        }

        public void Activate()
        {
            _player.ChangeDamageReductionPercent(_damageReductionPercent);

            _followingAbilityEffect = _vfxFactory.CreateEnergyShieldEffect(_player);
        }

        public void OnUpdate(float deltaTime)
        {
        }

        public void Deactivate()
        {
            _player.ChangeDamageReductionPercent(-_damageReductionPercent);

            _followingAbilityEffect.Clear();
            _followingAbilityEffect = null;
        }
    }
}