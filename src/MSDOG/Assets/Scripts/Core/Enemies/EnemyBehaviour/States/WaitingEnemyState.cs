namespace Core.Enemies.EnemyBehaviour.States
{
    public class WaitingEnemyState : IEnemyState
    {
        private readonly WandererBehaviourStateMachine _stateMachine;

        private float _timeTillStartWalking;

        public WaitingEnemyState(WandererBehaviourStateMachine stateMachine, float waitTime)
        {
            _stateMachine = stateMachine;
            _timeTillStartWalking = waitTime;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_timeTillStartWalking > 0f)
            {
                _timeTillStartWalking -= deltaTime;
                return;
            }

            _stateMachine.ChangeStateToWalking();
        }

        public void Dispose()
        {
        }
    }
}