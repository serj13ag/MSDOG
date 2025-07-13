using Core.Enemies.EnemyBehaviour.States;
using UnityEngine.AI;

namespace Core.Enemies.EnemyBehaviour
{
    public class MeleeBehaviourStateMachine : IEnemyStateMachine
    {
        private readonly IEnemyState _state;

        public MeleeBehaviourStateMachine(NavMeshAgent agent, Player player)
        {
            _state = new WalkingToPlayerEnemyState(agent, player);
        }

        public void OnUpdate(float deltaTime)
        {
            _state.OnUpdate(deltaTime);
        }
    }
}