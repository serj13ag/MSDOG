using UnityEngine;
using UnityEngine.AI;
using UtilityComponents;

namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public class WalkingToDestinationEnemyState : BaseTriggerAffectedEnemyState
    {
        private const float WalkingTimeout = 10f;

        private readonly WandererBehaviourStateMachine _stateMachine;
        private readonly AnimationBlock _animationBLock;
        private readonly NavMeshAgent _agent;

        private float _elapsedTime;

        public WalkingToDestinationEnemyState(Enemy enemy, WandererBehaviourStateMachine stateMachine,
            AnimationBlock animationBLock, NavMeshAgent agent, ColliderEventProvider triggerEnterProvider, Vector3 destination)
            : base(enemy, triggerEnterProvider)
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

        public override void Enter()
        {
            base.Enter();

            _animationBLock.SetRunning(true);
        }

        public override void OnUpdate(float deltaTime)
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

        public override void Exit()
        {
            base.Exit();

            _animationBLock.SetRunning(false);
        }
    }
}