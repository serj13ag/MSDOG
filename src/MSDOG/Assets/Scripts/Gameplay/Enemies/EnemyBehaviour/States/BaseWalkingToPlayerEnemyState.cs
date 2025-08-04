using UtilityComponents;

namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public abstract class BaseWalkingToPlayerEnemyState : BaseTriggerAffectedEnemyState
    {
        private readonly Enemy _enemy;
        private readonly AnimationBlock _animationBlock;

        protected Enemy Enemy => _enemy;

        protected BaseWalkingToPlayerEnemyState(Enemy enemy, AnimationBlock animationBlock,
            ColliderEventProvider triggerEnterProvider)
            : base(enemy, triggerEnterProvider)
        {
            _enemy = enemy;
            _animationBlock = animationBlock;
        }

        public override void Enter()
        {
            base.Enter();

            _animationBlock.SetRunning(true);
        }

        public override void OnUpdate(float deltaTime)
        {
            var enemyAgent = _enemy.Agent;
            if (!enemyAgent.isActiveAndEnabled)
            {
                return;
            }

            enemyAgent.SetDestination(_enemy.Player.transform.position);
        }

        public override void Exit()
        {
            base.Exit();

            _animationBlock.SetRunning(false);
        }
    }
}