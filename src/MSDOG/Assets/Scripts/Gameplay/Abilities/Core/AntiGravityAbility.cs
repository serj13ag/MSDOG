using Core.Models.Data;
using Gameplay.Interfaces;

namespace Gameplay.Abilities.Core
{
    public class AntiGravityAbility : BasePersistentAbility
    {
        private readonly IEntityWithAdditionalSpeed _entityWithAdditionalSpeed;
        private readonly float _speed;

        public AntiGravityAbility(AbilityData abilityData, IEntityWithAdditionalSpeed entityWithAdditionalSpeed)
        {
            _entityWithAdditionalSpeed = entityWithAdditionalSpeed;
            _speed = abilityData.Speed;
        }

        protected override void Activated()
        {
            _entityWithAdditionalSpeed.ChangeAdditionalSpeed(_speed);
        }

        protected override void Deactivated()
        {
            _entityWithAdditionalSpeed.ChangeAdditionalSpeed(-_speed);
        }
    }
}