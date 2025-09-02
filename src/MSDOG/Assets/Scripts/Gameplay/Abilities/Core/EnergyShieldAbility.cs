using Core.Models.Data;
using Gameplay.Interfaces;

namespace Gameplay.Abilities.Core
{
    public class EnergyShieldAbility : BasePersistentAbility
    {
        private readonly IEntityWithDamageReduction _entityWithDamageReduction;
        private readonly int _damageReductionPercent;

        public EnergyShieldAbility(AbilityData abilityData, IEntityWithDamageReduction entityWithDamageReduction)
        {
            _entityWithDamageReduction = entityWithDamageReduction;
            _damageReductionPercent = abilityData.DamageReductionPercent;
        }

        protected override void Activated()
        {
            _entityWithDamageReduction.ChangeDamageReductionPercent(_damageReductionPercent);
        }

        protected override void Deactivated()
        {
            _entityWithDamageReduction.ChangeDamageReductionPercent(-_damageReductionPercent);
        }
    }
}