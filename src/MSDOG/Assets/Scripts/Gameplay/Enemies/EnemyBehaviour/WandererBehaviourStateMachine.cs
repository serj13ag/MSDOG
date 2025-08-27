using Gameplay.Enemies.EnemyBehaviour.States;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.EnemyBehaviour
{
    public class WandererBehaviourStateMachine : BaseBehaviourStateMachine, IEnemyStateMachine
    {
        private const float SpawnTime = 1.4f;

        private const int NumberOfAttemptsToFindDestination = 30;
        private const float MinWalkingRadiusFromPlayer = 5f;
        private const float MaxWalkingRadiusFromPlayer = 15f;

        private readonly EnemyBehaviourStateMachineContext _context;

        public WandererBehaviourStateMachine(EnemyBehaviourStateMachineContext context)
        {
            _context = context;

            State = new SpawningEnemyState(this, SpawnTime);
        }

        public override void ChangeStateToPostSpawn()
        {
            ChangeStateToWaiting();
        }

        public void ChangeStateToWaiting()
        {
            var waitTime = Random.Range(1f, 3f);
            ChangeState(new WaitingEnemyState(this, _context, waitTime));
        }

        public void ChangeStateToWalking()
        {
            var destination = GetRandomDestinationNearPlayer();
            ChangeState(new WalkingToDestinationEnemyState(this, _context, destination));
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
            return randomPositionInsideCircle + _context.Player.transform.position;
        }
    }
}