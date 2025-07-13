namespace Core
{
    public class PlayerDamageBlock
    {
        private const float TakeDamageCooldown = 0.2f;

        private readonly HealthBlock _healthBlock;

        private float _timeTillTakeDamage;
        private int _accumulatedDamage;

        public PlayerDamageBlock(HealthBlock healthBlock)
        {
            _healthBlock = healthBlock;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_timeTillTakeDamage > 0f)
            {
                _timeTillTakeDamage -= deltaTime;
                return;
            }

            TakeDamage();
            _timeTillTakeDamage = TakeDamageCooldown;
        }

        public void RegisterDamage(int damage)
        {
            if (damage <= 0)
            {
                return;
            }

            _accumulatedDamage += damage;
        }

        private void TakeDamage()
        {
            _healthBlock.ReduceHealth(_accumulatedDamage);
            _accumulatedDamage = 0;
        }
    }
}