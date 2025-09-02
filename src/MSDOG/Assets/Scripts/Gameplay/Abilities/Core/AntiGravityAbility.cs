using Core.Models.Data;
using Gameplay.Interfaces;

namespace Gameplay.Abilities.Core
{
    public class AntiGravityAbility : BasePersistentAbility
    {
        private readonly IEntityWithAdditionalMoveSpeed _entityWithAdditionalMoveSpeed;
        private readonly float _speed;

        public AntiGravityAbility(AbilityData abilityData, IEntityWithAdditionalMoveSpeed entityWithAdditionalMoveSpeed)
        {
            _entityWithAdditionalMoveSpeed = entityWithAdditionalMoveSpeed;
            _speed = abilityData.Speed;
        }

        protected override void Activated()
        {
            _entityWithAdditionalMoveSpeed.ChangeAdditionalSpeed(_speed);
        }

        protected override void Deactivated()
        {
            _entityWithAdditionalMoveSpeed.ChangeAdditionalSpeed(-_speed);
        }
    }
}