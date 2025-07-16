using UtilityComponents;

namespace Core.Enemies.EnemyBehaviour.States
{
    public class MeleeWalkingToPlayerEnemyState : BaseWalkingToPlayerEnemyState
    {
        public MeleeWalkingToPlayerEnemyState(Enemy enemy, ColliderEventProvider triggerEnterProvider)
            : base(enemy, triggerEnterProvider)
        {
        }
    }
}