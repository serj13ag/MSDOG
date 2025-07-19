using Data;

namespace Core.Abilities
{
    public class EnergyShieldAbility : IAbility
    {
        private readonly Player _player;
        private readonly int _damageReductionPercent;

        public EnergyShieldAbility(AbilityData abilityData, Player player)
        {
            _player = player;
            _damageReductionPercent = abilityData.DamageReductionPercent;
        }

        public void Activate()
        {
            _player.ChangeDamageReductionPercent(_damageReductionPercent);
        }

        public void OnUpdate(float deltaTime)
        {
        }

        public void Deactivate()
        {
            _player.ChangeDamageReductionPercent(-_damageReductionPercent);
        }
    }
}