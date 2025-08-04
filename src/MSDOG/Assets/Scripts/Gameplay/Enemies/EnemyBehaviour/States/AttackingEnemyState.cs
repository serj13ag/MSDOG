using UtilityComponents;

namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public class AttackingEnemyState : BaseTriggerAffectedEnemyState
    {
        private readonly IEnemyStateMachine _stateMachine;
        private readonly Enemy _enemy;
        private readonly AnimationBlock _animationBlock;

        private float _timeTillSpawnEnd;

        public AttackingEnemyState(IEnemyStateMachine stateMachine, Enemy enemy, AnimationBlock animationBlock,
            ColliderEventProvider triggerEnterProvider, float spawnTime)
            : base(enemy, triggerEnterProvider)
        {
            _stateMachine = stateMachine;
            _enemy = enemy;
            _animationBlock = animationBlock;
            _timeTillSpawnEnd = spawnTime;
        }

        public override void Enter()
        {
            base.Enter();

            _enemy.transform.LookAt(_enemy.Player.transform);
            _animationBlock.TriggerAttack();
            _enemy.Agent.ResetPath();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_timeTillSpawnEnd > 0f)
            {
                _timeTillSpawnEnd -= deltaTime;
                return;
            }

            _stateMachine.ChangeStateToPostSpawn();
        }
    }
}