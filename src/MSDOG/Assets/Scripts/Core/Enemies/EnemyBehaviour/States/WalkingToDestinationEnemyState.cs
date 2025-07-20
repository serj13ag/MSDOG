using UnityEngine;
using UnityEngine.AI;

namespace Core.Enemies.EnemyBehaviour.States
{
    public class WalkingToDestinationEnemyState : IEnemyState
    {
        private const float WalkingTimeout = 10f;

        private readonly WandererBehaviourStateMachine _stateMachine;
        private readonly AnimationBlock _animationBLock;
        private readonly NavMeshAgent _agent;

        private float _elapsedTime;

        public WalkingToDestinationEnemyState(WandererBehaviourStateMachine stateMachine, AnimationBlock animationBLock,
            NavMeshAgent agent, Vector3 destination)
        {
            _stateMachine = stateMachine;
            _animationBLock = animationBLock;
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

        public void Enter()
        {
            _animationBLock.SetRunning(true);
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

        public void Exit()
        {
            _animationBLock.SetRunning(false);
        }

        public void Dispose()
        {
        }
    }
}