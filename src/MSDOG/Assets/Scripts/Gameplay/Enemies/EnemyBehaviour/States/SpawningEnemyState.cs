namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public class SpawningEnemyState : IEnemyState
    {
        private readonly IEnemyStateMachine _stateMachine;

        private float _timeTillSpawnEnd;

        public SpawningEnemyState(IEnemyStateMachine stateMachine, float spawnTime)
        {
            _stateMachine = stateMachine;
            _timeTillSpawnEnd = spawnTime;
        }

        public void Enter()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            if (_timeTillSpawnEnd > 0f)
            {
                _timeTillSpawnEnd -= deltaTime;
                return;
            }

            _stateMachine.ChangeStateToPostSpawn();
        }

        public void Exit()
        {
        }

        public void Dispose()
        {
        }
    }
}