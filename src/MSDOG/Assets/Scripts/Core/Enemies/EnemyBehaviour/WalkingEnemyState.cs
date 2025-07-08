using UnityEngine;
using UnityEngine.AI;

namespace Core.Enemies.EnemyBehaviour
{
    public class WalkingEnemyState : IEnemyState
    {
        private const float WalkingTimeout = 10f;

        private readonly Enemy _enemy;
        private readonly NavMeshAgent _agent;

        private float _elapsedTime;

        public WalkingEnemyState(Enemy enemy, NavMeshAgent agent, Vector3 destination)
        {
            _enemy = enemy;
            _agent = agent;

            var path = new NavMeshPath();
            if (agent.CalculatePath(destination, path))
            {
                agent.SetPath(path);
            }
            else
            {
                Debug.LogWarning($"Enemy {enemy.name} could not calculate path to {destination}");
            }
        }

        public void OnUpdate(float deltaTime)
        {
            _elapsedTime += deltaTime;
            if (_elapsedTime > WalkingTimeout)
            {
                _enemy.ChangeStateToWaiting();
                return;
            }

            var hasReachedDestination = !_agent.pathPending && _agent.remainingDistance < 0.5f;
            if (hasReachedDestination)
            {
                _enemy.ChangeStateToWaiting();
            }
        }
    }
}