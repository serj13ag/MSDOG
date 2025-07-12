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
        [SerializeField] private float _minWalkingRadiusFromPlayer = 5f;
        [SerializeField] private float _maxWalkingRadiusFromPlayer = 15f;

        private UpdateService _updateService;
        private Player _player;

        private IEnemyState _state;

        private HealthBlock _healthBlock;

        public event Action<Enemy> OnDestroyed;

        public void Init(UpdateService updateService, Player player)
        {
            _player = player;
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
            var destination = GetRandomDestinationNearPlayer();
            _state = new WalkingEnemyState(this, _agent, destination);
        }

        private Vector3 GetRandomDestinationNearPlayer()
        {
            for (var i = 0; i < NumberOfAttemptsToFindDestination; i++)
            {
                var positionNearPlayer = GetPositionNearPlayer();
                if (NavMesh.SamplePosition(positionNearPlayer, out var hit, 50f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            Debug.LogWarning($"Enemy {gameObject.name} could not find valid wander point");
            return Vector3.zero;
        }

        private Vector3 GetPositionNearPlayer()
        {
            // Random angle in radians
            var angle = Random.Range(0f, Mathf.PI * 2f);

            // Random radius between min and max, with square root to ensure uniform distribution
            var radius = Mathf.Sqrt(Random.Range(_minWalkingRadiusFromPlayer * _minWalkingRadiusFromPlayer,
                _maxWalkingRadiusFromPlayer * _maxWalkingRadiusFromPlayer));

            // Convert polar coordinates to Cartesian (X-Z plane)
            var x = Mathf.Cos(angle) * radius;
            var z = Mathf.Sin(angle) * radius;

            var randomPositionInsideCircle = new Vector3(x, 0f, z);
            return randomPositionInsideCircle + _player.transform.position;
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);

            OnDestroyed?.Invoke(this);
        }
    }
}