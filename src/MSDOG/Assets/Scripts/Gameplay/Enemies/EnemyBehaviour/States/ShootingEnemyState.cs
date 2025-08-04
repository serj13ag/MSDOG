using Constants;
using UnityEngine;
using UtilityComponents;

namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public class ShootingEnemyState : BaseTriggerAffectedEnemyState
    {
        private readonly RangeBehaviourStateMachine _stateMachine;
        private readonly Enemy _enemy;

        private float _timeTillShoot;

        public ShootingEnemyState(RangeBehaviourStateMachine stateMachine, Enemy enemy,
            ColliderEventProvider triggerEnterProvider, float timeTillShoot)
            : base(enemy, triggerEnterProvider)
        {
            _stateMachine = stateMachine;
            _enemy = enemy;

            _timeTillShoot = timeTillShoot;
        }

        public override void OnUpdate(float deltaTime)
        {
            _enemy.transform.LookAt(_enemy.Player.transform);

            if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) >
                Settings.Enemy.RangeCloseDistanceOut)
            {
                _stateMachine.ChangeStateToWalking(_timeTillShoot);
            }

            if (_timeTillShoot > 0f)
            {
                _timeTillShoot -= deltaTime;
                return;
            }

            _enemy.Shoot();
            _timeTillShoot = _enemy.Cooldown;
        }
    }
}