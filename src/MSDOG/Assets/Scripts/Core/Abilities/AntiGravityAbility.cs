using Data;

namespace Core.Abilities
{
    public class AntiGravityAbility : IAbility
    {
        private readonly Player _player;
        private readonly float _speed;

        public AntiGravityAbility(AbilityData abilityData, Player player)
        {
            _player = player;
            _speed = abilityData.Speed;
        }

        public void Activate()
        {
            _player.ChangeAdditionalSpeed(_speed);
        }

        public void OnUpdate(float deltaTime)
        {
        }

        public void Deactivate()
        {
            _player.ChangeAdditionalSpeed(-_speed);
        }
    }
}