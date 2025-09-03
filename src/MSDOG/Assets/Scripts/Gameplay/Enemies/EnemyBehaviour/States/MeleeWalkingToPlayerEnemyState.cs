using UnityEngine;

namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public class MeleeWalkingToPlayerEnemyState : BaseWalkingToPlayerEnemyState
    {
        private const float DistanceToAttack = 4f;

        private readonly MeleeBehaviourStateMachine _stateMachine;
        private readonly EnemyBehaviourStateMachineContext _context;

        public MeleeWalkingToPlayerEnemyState(MeleeBehaviourStateMachine stateMachine, EnemyBehaviourStateMachineContext context)
            : base(context)
        {
            _stateMachine = stateMachine;
            _context = context;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            var distanceToPlayer = Vector3.Distance(_context.Enemy.transform.position, _context.Target.GetPosition());
            if (distanceToPlayer < DistanceToAttack)
            {
                _stateMachine.ChangeStateToAttacking();
            }
        }
    }
}