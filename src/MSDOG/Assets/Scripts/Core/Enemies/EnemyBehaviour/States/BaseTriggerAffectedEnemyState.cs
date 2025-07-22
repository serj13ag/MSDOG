using Helpers;
using UnityEngine;
using UtilityComponents;

namespace Core.Enemies.EnemyBehaviour.States
{
    public abstract class BaseTriggerAffectedEnemyState : IEnemyState
    {
        private readonly Enemy _enemy;
        private readonly ColliderEventProvider _triggerEnterProvider;

        protected BaseTriggerAffectedEnemyState(Enemy enemy, ColliderEventProvider triggerEnterProvider)
        {
            _enemy = enemy;
            _triggerEnterProvider = triggerEnterProvider;

            if (triggerEnterProvider)
            {
                triggerEnterProvider.OnTriggerEntered += OnTriggerEntered;
                triggerEnterProvider.OnTriggerExited += OnTriggerExited;
            }
        }

        public virtual void Enter()
        {
        }

        public abstract void OnUpdate(float deltaTime);

        public virtual void Exit()
        {
            RemoveDamager();
        }

        private void OnTriggerEntered(Collider obj)
        {
            if (obj.gameObject.TryGetComponentInHierarchy<Player>(out var player))
            {
                player.RegisterDamager(_enemy.Id, _enemy.Damage);
            }
        }

        private void OnTriggerExited(Collider obj)
        {
            if (obj.gameObject.TryGetComponentInHierarchy<Player>(out var player))
            {
                player.RemoveDamager(_enemy.Id);
            }
        }

        private void RemoveDamager()
        {
            if (_triggerEnterProvider)
            {
                _enemy.Player.RemoveDamager(_enemy.Id);

                _triggerEnterProvider.OnTriggerEntered -= OnTriggerEntered;
                _triggerEnterProvider.OnTriggerExited -= OnTriggerExited;
            }
        }

        public void Dispose()
        {
            RemoveDamager();
        }
    }
}