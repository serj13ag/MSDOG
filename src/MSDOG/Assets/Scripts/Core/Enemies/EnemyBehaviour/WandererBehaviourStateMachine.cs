using Core.Enemies.EnemyBehaviour.States;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Enemies.EnemyBehaviour
{
    public class WandererBehaviourStateMachine : IEnemyStateMachine
    {
        private const float SpawnTime = 1.4f;

        private const int NumberOfAttemptsToFindDestination = 30;
        private const float MinWalkingRadiusFromPlayer = 5f;
        private const float MaxWalkingRadiusFromPlayer = 15f;

        private readonly NavMeshAgent _agent;
        private readonly Player _player;

        private IEnemyState _state;

        public WandererBehaviourStateMachine(Enemy enemy)
        {
            _agent = enemy.Agent;
            _player = enemy.Player;

            _state = new SpawningEnemyState(this, SpawnTime);
        }

        public void OnUpdate(float deltaTime)
        {
            _state.OnUpdate(deltaTime);
        }

        public void ChangeStateToPostSpawn()
        {
            ChangeStateToWaiting();
        }

        public void ChangeStateToWaiting()
        {
            var waitTime = Random.Range(1f, 3f);
            _state = new WaitingEnemyState(this, waitTime);
        }

        public void ChangeStateToWalking()
        {
            var destination = GetRandomDestinationNearPlayer();
            _state = new WalkingToDestinationEnemyState(this, _agent, destination);
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

            Debug.LogWarning($"{GetType().Name}: Could not find valid wander point");
            return Vector3.zero;
        }

        private Vector3 GetPositionNearPlayer()
        {
            // Random angle in radians
            var angle = Random.Range(0f, Mathf.PI * 2f);

            // Random radius between min and max, with square root to ensure uniform distribution
            var radius = Mathf.Sqrt(Random.Range(MinWalkingRadiusFromPlayer * MinWalkingRadiusFromPlayer,
                MaxWalkingRadiusFromPlayer * MaxWalkingRadiusFromPlayer));

            // Convert polar coordinates to Cartesian (X-Z plane)
            var x = Mathf.Cos(angle) * radius;
            var z = Mathf.Sin(angle) * radius;

            var randomPositionInsideCircle = new Vector3(x, 0f, z);
            return randomPositionInsideCircle + _player.transform.position;
        }

        public void Dispose()
        {
            _state.Dispose();
        }
    }
}