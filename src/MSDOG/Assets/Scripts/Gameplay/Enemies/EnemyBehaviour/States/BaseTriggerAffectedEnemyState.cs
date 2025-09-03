using Gameplay.Interfaces;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public abstract class BaseTriggerAffectedEnemyState : IEnemyState
    {
        private readonly EnemyBehaviourStateMachineContext _context;

        private IOverlapDamageableEntity _damageable;

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
            if (obj.gameObject.TryGetComponentInHierarchy<IOverlapDamageableEntity>(out var damageable))
            {
                _damageable = damageable;
                var enemy = _context.Enemy;
                damageable.RegisterOverlapDamager(enemy.Id, enemy.Damage);
            }
        }

        private void OnTriggerExited(Collider obj)
        {
            if (obj.gameObject.TryGetComponentInHierarchy<IOverlapDamageableEntity>(out var damageable))
            {
                _damageable = null;
                damageable.RemoveOverlapDamager(_context.Enemy.Id);
            }
        }

        private void RemoveDamager()
        {
            var triggerEnterProvider = _context.DamagePlayerColliderTriggerEnterProvider;
            if (triggerEnterProvider)
            {
                _damageable?.RemoveOverlapDamager(_context.Enemy.Id);

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