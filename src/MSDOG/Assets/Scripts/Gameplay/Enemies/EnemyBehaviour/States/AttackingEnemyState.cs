using UnityEngine;

namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public class AttackingEnemyState : BaseTriggerAffectedEnemyState
    {
        private readonly IEnemyStateMachine _stateMachine;
        private readonly EnemyBehaviourStateMachineContext _context;

        private float _timeTillSpawnEnd;

        public AttackingEnemyState(IEnemyStateMachine stateMachine, EnemyBehaviourStateMachineContext context, float spawnTime)
            : base(context)
        {
            _stateMachine = stateMachine;
            _context = context;

            _timeTillSpawnEnd = spawnTime;
        }

        public override void Enter()
        {
            base.Enter();

            var enemy = _context.Enemy;
            var lookDirection = (_context.Target.GetPosition() - enemy.transform.position).normalized;
            enemy.transform.rotation = Quaternion.LookRotation(lookDirection);

            _context.Agent.ResetPath();
            _context.AnimationBlock.TriggerAttack();
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