using Gameplay.Enemies.EnemyBehaviour.States;
using Utility;

namespace Gameplay.Enemies.EnemyBehaviour
{
    public class RangeBehaviourStateMachine : BaseBehaviourStateMachine
    {
        private const float SpawnTime = 2f;

        private readonly Enemy _enemy;
        private readonly ColliderEventProvider _triggerEnterProvider;
        private readonly AnimationBlock _animationBlock;

        public RangeBehaviourStateMachine(Enemy enemy, ColliderEventProvider triggerEnterProvider)
        {
            _enemy = enemy;
            _triggerEnterProvider = triggerEnterProvider;
            _animationBlock = enemy.AnimationBlock;

            State = new SpawningEnemyState(this, SpawnTime);
        }

        public override void ChangeStateToPostSpawn()
        {
            ChangeStateToWalking(0f);
        }

        public void ChangeStateToWalking(float timeTillShoot)
        {
            ChangeState(new RangeWalkingToPlayerEnemyState(this, _enemy, _animationBlock, _triggerEnterProvider, timeTillShoot));
        }

        public void ChangeStateToShooting(float timeTillShoot)
        {
            ChangeState(new ShootingEnemyState(this, _enemy, _triggerEnterProvider, timeTillShoot));
        }
    }
}