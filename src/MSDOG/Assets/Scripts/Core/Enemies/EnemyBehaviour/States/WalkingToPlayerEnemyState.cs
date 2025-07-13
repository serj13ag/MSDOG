using UnityEngine.AI;

namespace Core.Enemies.EnemyBehaviour.States
{
    public class WalkingToPlayerEnemyState : IEnemyState
    {
        private readonly NavMeshAgent _agent;
        private readonly Player _player;

        public WalkingToPlayerEnemyState(NavMeshAgent agent, Player player)
        {
            _agent = agent;
            _player = player;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_agent.isActiveAndEnabled)
            {
                return;
            }

            _agent.SetDestination(_player.transform.position);
        }
    }
}