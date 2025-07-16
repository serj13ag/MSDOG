using UnityEngine;
using UtilityComponents;

namespace Core.Enemies.EnemyBehaviour.States
{
    public abstract class BaseWalkingToPlayerEnemyState : IEnemyState
    {
        private readonly Enemy _enemy;
        private readonly ColliderEventProvider _triggerEnterProvider;

        protected Enemy Enemy => _enemy;

        protected BaseWalkingToPlayerEnemyState(Enemy enemy, ColliderEventProvider triggerEnterProvider)
        {
            _enemy = enemy;
            _triggerEnterProvider = triggerEnterProvider;

            if (triggerEnterProvider)
            {
                triggerEnterProvider.OnTriggerEntered += OnTriggerEntered;
                triggerEnterProvider.OnTriggerExited += OnTriggerExited;
            }
        }

        public virtual void OnUpdate(float deltaTime)
        {
            var enemyAgent = _enemy.Agent;
            if (!enemyAgent.isActiveAndEnabled)
            {
                return;
            }

            enemyAgent.SetDestination(_enemy.Player.transform.position);
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