using Constants;
using UnityEngine;
using UtilityComponents;

namespace Core.Enemies.EnemyBehaviour.States
{
    public class ShootingEnemyState : IEnemyState
    {
        private readonly RangeBehaviourStateMachine _stateMachine;
        private readonly Enemy _enemy;
        private readonly ColliderEventProvider _triggerEnterProvider;

        private float _timeTillShoot;

        public ShootingEnemyState(RangeBehaviourStateMachine stateMachine, Enemy enemy,
            ColliderEventProvider triggerEnterProvider, float timeTillShoot)
        {
            _triggerEnterProvider = triggerEnterProvider;
            _stateMachine = stateMachine;
            _enemy = enemy;

            _timeTillShoot = timeTillShoot;

            if (triggerEnterProvider)
            {
                triggerEnterProvider.OnTriggerEntered += OnTriggerEntered;
                triggerEnterProvider.OnTriggerExited += OnTriggerExited;
            }
        }

        public void Enter()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            _enemy.transform.LookAt(_enemy.Player.transform);

            if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) > Settings.Enemy.RangeCloseDistanceOut)
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

        public void Exit()
        {
        }

        private void OnTriggerEntered(Collider obj)
        {
            var player = obj.GetComponentInParent<Player>();
            if (!player)
            {
                return;
            }

            player.RegisterDamager(_enemy.Id, _enemy.Damage);
        }

        private void OnTriggerExited(Collider obj)
        {
            var player = obj.GetComponentInParent<Player>();
            if (!player)
            {
                return;
            }

            player.RemoveDamager(_enemy.Id);
        }

        public void Dispose()
        {
            if (_triggerEnterProvider)
            {
                _enemy.Player.RemoveDamager(_enemy.Id);

                _triggerEnterProvider.OnTriggerEntered -= OnTriggerEntered;
                _triggerEnterProvider.OnTriggerExited -= OnTriggerExited;
            }
        }
    }
}