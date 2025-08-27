namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public class WaitingEnemyState : BaseTriggerAffectedEnemyState
    {
        private readonly WandererBehaviourStateMachine _stateMachine;

        private float _timeTillStartWalking;

        public WaitingEnemyState(WandererBehaviourStateMachine stateMachine, EnemyBehaviourStateMachineContext context, float waitTime)
            : base(context)
        {
            _stateMachine = stateMachine;
            _timeTillStartWalking = waitTime;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_timeTillStartWalking > 0f)
            {
                _timeTillStartWalking -= deltaTime;
                return;
            }

            _stateMachine.ChangeStateToWalking();
        }
    }
}