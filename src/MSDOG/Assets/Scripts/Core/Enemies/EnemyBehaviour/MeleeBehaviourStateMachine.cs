using Core.Enemies.EnemyBehaviour.States;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Enemies.EnemyBehaviour
{
    public class MeleeBehaviourStateMachine : IEnemyStateMachine
    {
        private readonly int _damage;
        private readonly IEnemyState _state;

        public MeleeBehaviourStateMachine(NavMeshAgent agent, int damage, Player player)
        {
            _damage = damage;
            _state = new WalkingToPlayerEnemyState(agent, player);
        }

        public void OnUpdate(float deltaTime)
        {
            _state.OnUpdate(deltaTime);
        }

        public void OnTriggerEntered(Collider collider)
        {
            var player = collider.GetComponentInParent<Player>();
            if (!player)
            {
                return;
            }

            player.RegisterDamage(_damage);
        }
    }
}