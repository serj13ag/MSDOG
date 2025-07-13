using System;
using Core.Enemies.EnemyBehaviour;
using Data;
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

        private UpdateService _updateService;

        private IEnemyStateMachine _stateMachine;

        private HealthBlock _healthBlock;

        public event Action<Enemy> OnDied;

        public void Init(UpdateService updateService, Player player, EnemyData data)
        {
            _updateService = updateService;

            _agent.speed = data.Speed;

            _healthBlock = new HealthBlock(data.MaxHealth);
            _healthBarDebugView.Init(_healthBlock);

            _stateMachine = data.Type switch
            {
                EnemyType.Wanderer => new WandererBehaviourStateMachine(_agent, player),
                EnemyType.Melee => new MeleeBehaviourStateMachine(_agent, player),
                _ => throw new ArgumentOutOfRangeException(nameof(data.Type), data.Type, null),
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
                OnDied?.Invoke(this);
            }
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);
        }
    }
}