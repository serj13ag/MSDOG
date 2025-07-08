namespace Core.Enemies.EnemyBehaviour
{
    public class WaitingEnemyState : IEnemyState
    {
        private readonly Enemy _enemy;

        private float _timeTillStartWalking;

        public WaitingEnemyState(Enemy enemy, float waitTime)
        {
            _enemy = enemy;

            _timeTillStartWalking = waitTime;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_timeTillStartWalking > 0f)
            {
                _timeTillStartWalking -= deltaTime;
                return;
            }

            _enemy.ChangeStateToWalking();
        }
    }
}