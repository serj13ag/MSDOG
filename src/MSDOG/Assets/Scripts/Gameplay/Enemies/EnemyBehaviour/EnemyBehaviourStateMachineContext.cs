using Gameplay.Providers;
using UnityEngine.AI;
using Utility;

namespace Gameplay.Enemies.EnemyBehaviour
{
    public class EnemyBehaviourStateMachineContext
    {
        public Enemy Enemy { get; }
        public NavMeshAgent Agent { get; }
        public AnimationBlock AnimationBlock { get; }
        public ColliderEventProvider DamagePlayerColliderTriggerEnterProvider { get; }
        public Player Player { get; }

        public EnemyBehaviourStateMachineContext(Enemy enemy, NavMeshAgent agent, AnimationBlock animationBlock,
            ColliderEventProvider damagePlayerColliderTriggerEnterProvider, IPlayerProvider playerProvider)
        {
            Enemy = enemy;
            Agent = agent;
            AnimationBlock = animationBlock;
            DamagePlayerColliderTriggerEnterProvider = damagePlayerColliderTriggerEnterProvider;
            Player = playerProvider.Player;
        }
    }
}