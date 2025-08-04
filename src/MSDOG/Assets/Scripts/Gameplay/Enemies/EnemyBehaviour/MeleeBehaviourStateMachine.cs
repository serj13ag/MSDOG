using Gameplay.Enemies.EnemyBehaviour.States;
using UtilityComponents;

namespace Gameplay.Enemies.EnemyBehaviour
{
    public class MeleeBehaviourStateMachine : BaseBehaviourStateMachine
    {
        private const float SpawnTime = 1.3f;

        private readonly Enemy _enemy;
        private readonly ColliderEventProvider _triggerEnterProvider;
        private readonly AnimationBlock _animationBlock;

        public MeleeBehaviourStateMachine(Enemy enemy, ColliderEventProvider triggerEnterProvider)
        {
            _enemy = enemy;
            _triggerEnterProvider = triggerEnterProvider;
            _animationBlock = enemy.AnimationBlock;

            State = new SpawningEnemyState(this, SpawnTime);
        }

        public override void ChangeStateToPostSpawn()
        {
            ChangeState(new MeleeWalkingToPlayerEnemyState(this, _enemy, _animationBlock, _triggerEnterProvider));
        }

        public void ChangeStateToAttacking()
        {
            ChangeState(new AttackingEnemyState(this, _enemy, _animationBlock, _triggerEnterProvider, 2f));
        }
    }
}