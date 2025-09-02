using Core.Controllers;
using Core.Models.Data;

namespace Gameplay.Abilities
{
    public class EnergyShieldAbility : BasePersistentAbility
    {
        private readonly Player _player;
        private readonly int _damageReductionPercent;

        public EnergyShieldAbility(AbilityData abilityData, Player player, ISoundController soundController)
            : base(abilityData, soundController)
        {
            _player = player;
            _damageReductionPercent = abilityData.DamageReductionPercent;
        }

        protected override void Activated()
        {
            _player.ChangeDamageReductionPercent(_damageReductionPercent);
        }

        protected override void Deactivated()
        {
            _player.ChangeDamageReductionPercent(-_damageReductionPercent);
        }
    }
}