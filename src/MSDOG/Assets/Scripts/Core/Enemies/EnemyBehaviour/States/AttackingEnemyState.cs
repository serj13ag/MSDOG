using Helpers;
using UnityEngine;
using UtilityComponents;

namespace Core.Enemies.EnemyBehaviour.States
{
    public class AttackingEnemyState : IEnemyState
    {
        private readonly IEnemyStateMachine _stateMachine;
        private readonly Enemy _enemy;
        private readonly AnimationBlock _animationBlock;
        private readonly ColliderEventProvider _triggerEnterProvider;

        private float _timeTillSpawnEnd;

        public AttackingEnemyState(IEnemyStateMachine stateMachine, Enemy enemy, AnimationBlock animationBlock,
            ColliderEventProvider triggerEnterProvider, float spawnTime)
        {
            _stateMachine = stateMachine;
            _enemy = enemy;
            _animationBlock = animationBlock;
            _triggerEnterProvider = triggerEnterProvider;
            _timeTillSpawnEnd = spawnTime;

            if (triggerEnterProvider)
            {
                triggerEnterProvider.OnTriggerEntered += OnTriggerEntered;
                triggerEnterProvider.OnTriggerExited += OnTriggerExited;
            }
        }

        public void Enter()
        {
            _enemy.transform.LookAt(_enemy.Player.transform);
            _animationBlock.TriggerAttack();
            _enemy.Agent.ResetPath();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_timeTillSpawnEnd > 0f)
            {
                _timeTillSpawnEnd -= deltaTime;
                return;
            }

            _stateMachine.ChangeStateToPostSpawn();
        }

        public void Exit()
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