using Common;
using UnityEngine;

namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public class ShootingEnemyState : BaseTriggerAffectedEnemyState
    {
        private readonly RangeBehaviourStateMachine _stateMachine;
        private readonly EnemyBehaviourStateMachineContext _context;

        private float _timeTillShoot;

        public ShootingEnemyState(RangeBehaviourStateMachine stateMachine, EnemyBehaviourStateMachineContext context)
            : base(context)
        {
            _stateMachine = stateMachine;
            _context = context;
        }

        public override void Enter()
        {
            base.Enter();

            _timeTillShoot = 0f;
        }

        public override void OnUpdate(float deltaTime)
        {
            var targetPosition = _context.Target.GetPosition();

            var enemy = _context.Enemy;
            var lookDirection = (targetPosition - enemy.transform.position).normalized;
            enemy.transform.rotation = Quaternion.LookRotation(lookDirection);

            if (Vector3.Distance(enemy.transform.position, targetPosition) > Constants.Enemy.RangeCloseDistanceOut)
            {
                _stateMachine.ChangeStateToWalking();
            }

            if (_timeTillShoot > 0f)
            {
                _timeTillShoot -= deltaTime;
                return;
            }

            enemy.Shoot();
            _timeTillShoot = enemy.Cooldown;
        }
    }
}