using System;
using Core.Enemies.EnemyBehaviour;
using Data;
using Interfaces;
using Services;
using UI;
using UnityEngine;
using UnityEngine.AI;
using UtilityComponents;

namespace Core.Enemies
{
    public class Enemy : MonoBehaviour, IUpdatable
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private HealthBarDebugView _healthBarDebugView;
        [SerializeField] private ColliderEventProvider _triggerEnterProvider;

        private UpdateService _updateService;

        private Guid _id;
        private Player _player;
        private int _damage;
        private HealthBlock _healthBlock;
        private IEnemyStateMachine _stateMachine;

        public Guid Id => _id;
        public NavMeshAgent Agent => _agent;
        public Player Player => _player;
        public int Damage => _damage;

        public event Action<Enemy> OnDied;

        public void Init(UpdateService updateService, Player player, EnemyData data)
        {
            _updateService = updateService;

            _id = Guid.NewGuid();
            _player = player;
            _damage = data.Damage;

            _healthBlock = new HealthBlock(data.MaxHealth);
            _healthBarDebugView.Init(_healthBlock);

            _stateMachine = data.Type switch
            {
                EnemyType.Wanderer => new WandererBehaviourStateMachine(this),
                EnemyType.Melee => new MeleeBehaviourStateMachine(this, _triggerEnterProvider),
                _ => throw new ArgumentOutOfRangeException(nameof(data.Type), data.Type, null),
            };

            _agent.speed = data.Speed;

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
            _stateMachine.Dispose();

            _updateService.Remove(this);
        }
    }
}