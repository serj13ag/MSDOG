using UtilityComponents;

namespace Core.Enemies.EnemyBehaviour.States
{
    public class MeleeWalkingToPlayerEnemyState : BaseWalkingToPlayerEnemyState
    {
        public MeleeWalkingToPlayerEnemyState(Enemy enemy, AnimationBlock animationBlock,
            ColliderEventProvider triggerEnterProvider)
            : base(enemy, animationBlock, triggerEnterProvider)
        {
        }
    }
}