using Constants;
using UnityEngine;

namespace Core.Enemies.EnemyBehaviour.States
{
    public class ShootingEnemyState : IEnemyState
    {
        private readonly RangeBehaviourStateMachine _stateMachine;
        private readonly Enemy _enemy;

        private float _timeTillShoot;

        public ShootingEnemyState(RangeBehaviourStateMachine stateMachine, Enemy enemy, float timeTillShoot)
        {
            _stateMachine = stateMachine;
            _enemy = enemy;

            _timeTillShoot = timeTillShoot;
        }

        public void Enter()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) > Settings.Enemy.RangeCloseDistanceOut)
            {
                _stateMachine.ChangeStateToWalking(_timeTillShoot);
            }

            if (_timeTillShoot > 0f)
            {
                _timeTillShoot -= deltaTime;
                return;
            }

            _enemy.ShootProjectileToPlayer();
            _timeTillShoot = _enemy.Cooldown;
        }

        public void Exit()
        {
        }

        public void Dispose()
        {
        }
    }
}