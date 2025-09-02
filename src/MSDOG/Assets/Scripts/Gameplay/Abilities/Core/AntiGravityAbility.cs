using Core.Controllers;
using Core.Models.Data;

namespace Gameplay.Abilities.Core
{
    public class AntiGravityAbility : BasePersistentAbility
    {
        private readonly Player _player;
        private readonly float _speed;

        public AntiGravityAbility(AbilityData abilityData, Player player, ISoundController soundController)
            : base(abilityData, soundController)
        {
            _player = player;
            _speed = abilityData.Speed;
        }

        protected override void Activated()
        {
            _player.ChangeAdditionalSpeed(_speed);
        }

        protected override void Deactivated()
        {
            _player.ChangeAdditionalSpeed(-_speed);
        }
    }
}