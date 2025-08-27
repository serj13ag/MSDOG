namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public abstract class BaseWalkingToPlayerEnemyState : BaseTriggerAffectedEnemyState
    {
        private readonly EnemyBehaviourStateMachineContext _context;

        protected BaseWalkingToPlayerEnemyState(EnemyBehaviourStateMachineContext context)
            : base(context)
        {
            _context = context;
        }

        public override void Enter()
        {
            base.Enter();

            _context.AnimationBlock.SetRunning(true);
        }

        public override void OnUpdate(float deltaTime)
        {
            var enemyAgent = _context.Agent;
            if (!enemyAgent.isActiveAndEnabled)
            {
                return;
            }

            enemyAgent.SetDestination(_context.Player.transform.position);
        }

        public override void Exit()
        {
            base.Exit();

            _context.AnimationBlock.SetRunning(false);
        }
    }
}