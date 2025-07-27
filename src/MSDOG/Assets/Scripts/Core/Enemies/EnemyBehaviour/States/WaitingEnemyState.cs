using UtilityComponents;

namespace Core.Enemies.EnemyBehaviour.States
{
    public class WaitingEnemyState : BaseTriggerAffectedEnemyState
    {
        private readonly WandererBehaviourStateMachine _stateMachine;

        private float _timeTillStartWalking;

        public WaitingEnemyState(Enemy enemy, WandererBehaviourStateMachine stateMachine,
            ColliderEventProvider triggerEnterProvider, float waitTime)
            : base(enemy, triggerEnterProvider)
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