using Core.Enemies.EnemyBehaviour.States;
using UtilityComponents;

namespace Core.Enemies.EnemyBehaviour
{
    public class RangeBehaviourStateMachine : IEnemyStateMachine
    {
        private readonly Enemy _enemy;
        private readonly ColliderEventProvider _triggerEnterProvider;

        private IEnemyState _state;

        public RangeBehaviourStateMachine(Enemy enemy, ColliderEventProvider triggerEnterProvider)
        {
            _enemy = enemy;
            _triggerEnterProvider = triggerEnterProvider;

            _state = new RangeWalkingToPlayerEnemyState(this, enemy, triggerEnterProvider, 0f);
        }

        public void OnUpdate(float deltaTime)
        {
            _state.OnUpdate(deltaTime);
        }

        public void ChangeStateToWalking(float timeTillShoot)
        {
            _state = new RangeWalkingToPlayerEnemyState(this, _enemy, _triggerEnterProvider, timeTillShoot);
        }

        public void ChangeStateToShooting(float timeTillShoot)
        {
            _state = new ShootingEnemyState(this, _enemy, timeTillShoot);
        }

        public void Dispose()
        {
            _state.Dispose();
        }
    }
}