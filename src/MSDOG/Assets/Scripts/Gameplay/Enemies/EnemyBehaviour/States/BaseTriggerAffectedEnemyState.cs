using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public abstract class BaseTriggerAffectedEnemyState : IEnemyState
    {
        private readonly EnemyBehaviourStateMachineContext _context;

        protected BaseTriggerAffectedEnemyState(EnemyBehaviourStateMachineContext context)
        {
            _context = context;

            var triggerEnterProvider = _context.DamagePlayerColliderTriggerEnterProvider;
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
                var enemy = _context.Enemy;
                player.RegisterDamager(enemy.Id, enemy.Damage);
            }
        }

        private void OnTriggerExited(Collider obj)
        {
            if (obj.gameObject.TryGetComponentInHierarchy<Player>(out var player))
            {
                player.RemoveDamager(_context.Enemy.Id);
            }
        }

        private void RemoveDamager()
        {
            var triggerEnterProvider = _context.DamagePlayerColliderTriggerEnterProvider;
            if (triggerEnterProvider)
            {
                var enemy = _context.Enemy;
                _context.Player.RemoveDamager(enemy.Id);

                triggerEnterProvider.OnTriggerEntered -= OnTriggerEntered;
                triggerEnterProvider.OnTriggerExited -= OnTriggerExited;
            }
        }

        public void Dispose()
        {
            RemoveDamager();
        }
    }
}