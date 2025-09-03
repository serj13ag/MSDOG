using Common;
using UnityEngine;

namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public class RangeWalkingToPlayerEnemyState : BaseWalkingToPlayerEnemyState
    {
        private readonly RangeBehaviourStateMachine _stateMachine;
        private readonly EnemyBehaviourStateMachineContext _context;

        public RangeWalkingToPlayerEnemyState(RangeBehaviourStateMachine stateMachine, EnemyBehaviourStateMachineContext context)
            : base(context)
        {
            _stateMachine = stateMachine;
            _context = context;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            var distanceToEnemy = Vector3.Distance(_context.Enemy.GetPosition(), _context.Target.GetPosition());
            if (distanceToEnemy < Constants.Enemy.RangeCloseDistance)
            {
                _context.Agent.ResetPath();
                _stateMachine.ChangeStateToShooting();
            }
        }
    }
}