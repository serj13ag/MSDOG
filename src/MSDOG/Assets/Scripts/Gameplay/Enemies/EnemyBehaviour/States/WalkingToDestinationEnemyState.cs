using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public class WalkingToDestinationEnemyState : BaseTriggerAffectedEnemyState
    {
        private const float WalkingTimeout = 10f;

        private readonly WandererBehaviourStateMachine _stateMachine;
        private readonly EnemyBehaviourStateMachineContext _context;

        private float _elapsedTime;

        public WalkingToDestinationEnemyState(WandererBehaviourStateMachine stateMachine,
            EnemyBehaviourStateMachineContext context, Vector3 destination)
            : base(context)
        {
            _stateMachine = stateMachine;
            _context = context;

            var path = new NavMeshPath();
            var agent = context.Agent;
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

            _context.AnimationBlock.SetRunning(true);
        }

        public override void OnUpdate(float deltaTime)
        {
            var agent = _context.Agent;
            if (!agent.isActiveAndEnabled)
            {
                return;
            }

            _elapsedTime += deltaTime;
            if (_elapsedTime > WalkingTimeout)
            {
                _stateMachine.ChangeStateToWaiting();
                return;
            }

            var hasReachedDestination = !agent.pathPending && agent.remainingDistance < 0.5f;
            if (hasReachedDestination)
            {
                _stateMachine.ChangeStateToWaiting();
            }
        }

        public override void Exit()
        {
            base.Exit();

            _context.AnimationBlock.SetRunning(false);
        }
    }
}