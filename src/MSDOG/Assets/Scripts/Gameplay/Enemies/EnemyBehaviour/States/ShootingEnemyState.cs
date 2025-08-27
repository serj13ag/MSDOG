using Constants;
using UnityEngine;

namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public class ShootingEnemyState : BaseTriggerAffectedEnemyState
    {
        private readonly RangeBehaviourStateMachine _stateMachine;
        private readonly EnemyBehaviourStateMachineContext _context;

        private float _timeTillShoot; // TODO: refactor

        public ShootingEnemyState(RangeBehaviourStateMachine stateMachine, EnemyBehaviourStateMachineContext context,
            float timeTillShoot)
            : base(context)
        {
            _stateMachine = stateMachine;
            _context = context;

            _timeTillShoot = timeTillShoot;
        }

        public override void OnUpdate(float deltaTime)
        {
            var enemy = _context.Enemy;
            var player = _context.Player;

            enemy.transform.LookAt(player.transform);

            if (Vector3.Distance(enemy.transform.position, player.transform.position) >
                Settings.Enemy.RangeCloseDistanceOut)
            {
                _stateMachine.ChangeStateToWalking(_timeTillShoot);
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