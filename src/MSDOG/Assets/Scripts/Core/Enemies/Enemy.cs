using System;
using Core.Enemies.EnemyBehaviour;
using Interfaces;
using Services;
using UI;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Core.Enemies
{
    public class Enemy : MonoBehaviour, IUpdatable
    {
        private const int NumberOfAttemptsToFindDestination = 30;

        [SerializeField] private HealthBarDebugView _healthBarDebugView;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _walkingTargetRadius;

        private UpdateService _updateService;

        private IEnemyState _state;

        private HealthBlock _healthBlock;

        public event Action<Enemy> OnDestroyed;

        public void Init(UpdateService updateService)
        {
            _updateService = updateService;

            _healthBlock = new HealthBlock(_maxHealth);
            _healthBarDebugView.Init(_healthBlock);

            updateService.Register(this);

            ChangeStateToWaiting();
        }

        public void OnUpdate(float deltaTime)
        {
            _state.OnUpdate(deltaTime);
        }

        public void TakeDamage(int damage)
        {
            _healthBlock.ReduceHealth(damage);
            if (_healthBlock.HasZeroHealth)
            {
                Destroy(gameObject); // TODO: fix
            }
        }

        public void ChangeStateToWaiting()
        {
            _state = new WaitingEnemyState(this, Random.Range(1f, 3f));
        }

        public void ChangeStateToWalking()
        {
            var destination = GetRandomDestination();
            _state = new WalkingEnemyState(this, _agent, destination);
        }

        private Vector3 GetRandomDestination()
        {
            for (var i = 0; i < NumberOfAttemptsToFindDestination; i++)
            {
                var randomDirection = Random.insideUnitSphere * _walkingTargetRadius;
                randomDirection += transform.position;

                if (NavMesh.SamplePosition(randomDirection, out var hit, _walkingTargetRadius, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            Debug.LogWarning($"Enemy {gameObject.name} could not find valid wander point");
            return Vector3.zero;
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);

            OnDestroyed?.Invoke(this);
        }
    }
}