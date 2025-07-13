using System;
using Core.Enemies.EnemyBehaviour;
using Interfaces;
using Services;
using UI;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Enemies
{
    public class Enemy : MonoBehaviour, IUpdatable
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private HealthBarDebugView _healthBarDebugView;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private int _maxHealth;

        private UpdateService _updateService;

        private IEnemyStateMachine _stateMachine;

        private HealthBlock _healthBlock;

        public event Action<Enemy> OnDestroyed;

        public void Init(UpdateService updateService, Player player, EnemyType type)
        {
            _updateService = updateService;

            _healthBlock = new HealthBlock(_maxHealth);
            _healthBarDebugView.Init(_healthBlock);

            _stateMachine = type switch
            {
                EnemyType.Wanderer => new WandererBehaviourStateMachine(_agent, player),
                EnemyType.Melee => new MeleeBehaviourStateMachine(_agent, player),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };

            updateService.Register(this);
        }

        public void OnUpdate(float deltaTime)
        {
            _stateMachine.OnUpdate(deltaTime);
        }

        public void TakeDamage(int damage)
        {
            _healthBlock.ReduceHealth(damage);
            if (_healthBlock.HasZeroHealth)
            {
                Destroy(gameObject); // TODO: fix
            }
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);

            OnDestroyed?.Invoke(this);
        }
    }
}