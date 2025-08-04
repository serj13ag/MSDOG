using UnityEngine;
using UtilityComponents;

namespace Gameplay.Enemies.EnemyBehaviour.States
{
    public class MeleeWalkingToPlayerEnemyState : BaseWalkingToPlayerEnemyState
    {
        private const float DistanceToAttack = 4f;

        private readonly MeleeBehaviourStateMachine _stateMachine;

        public MeleeWalkingToPlayerEnemyState(MeleeBehaviourStateMachine stateMachine, Enemy enemy, AnimationBlock animationBlock,
            ColliderEventProvider triggerEnterProvider)
            : base(enemy, animationBlock, triggerEnterProvider)
        {
            _stateMachine = stateMachine;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            var distanceToPLayer = Vector3.Distance(Enemy.transform.position, Enemy.Player.transform.position);
            if (distanceToPLayer < DistanceToAttack)
            {
                _stateMachine.ChangeStateToAttacking();
            }
        }
    }
}