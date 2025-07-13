using Core.Enemies.EnemyBehaviour.Wanderer;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Enemies.EnemyBehaviour
{
    public class WalkingEnemyState : IEnemyState
    {
        private const float WalkingTimeout = 10f;

        private readonly WandererBehaviourStateMachine _stateMachine;
        private readonly NavMeshAgent _agent;

        private float _elapsedTime;

        public WalkingEnemyState(WandererBehaviourStateMachine stateMachine, NavMeshAgent agent, Vector3 destination)
        {
            _stateMachine = stateMachine;
            _agent = agent;

            var path = new NavMeshPath();
            if (agent.CalculatePath(destination, path))
            {
                agent.SetPath(path);
            }
            else
            {
                Debug.LogWarning($"{GetType().Name}: Could not calculate path to {destination}");
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_agent.isActiveAndEnabled)
            {
                return;
            }

            _elapsedTime += deltaTime;
            if (_elapsedTime > WalkingTimeout)
            {
                _stateMachine.ChangeStateToWaiting();
                return;
            }

            var hasReachedDestination = !_agent.pathPending && _agent.remainingDistance < 0.5f;
            if (hasReachedDestination)
            {
                _stateMachine.ChangeStateToWaiting();
            }
        }
    }
}