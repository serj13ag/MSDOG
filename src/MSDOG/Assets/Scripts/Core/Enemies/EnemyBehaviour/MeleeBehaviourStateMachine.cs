using Core.Enemies.EnemyBehaviour.States;
using UtilityComponents;

namespace Core.Enemies.EnemyBehaviour
{
    public class MeleeBehaviourStateMachine : IEnemyStateMachine
    {
        private const float SpawnTime = 1.3f;

        private readonly Enemy _enemy;
        private readonly ColliderEventProvider _triggerEnterProvider;

        private IEnemyState _state;

        public MeleeBehaviourStateMachine(Enemy enemy, ColliderEventProvider triggerEnterProvider)
        {
            _enemy = enemy;
            _triggerEnterProvider = triggerEnterProvider;

            _state = new SpawningEnemyState(this, SpawnTime);
        }


        public void OnUpdate(float deltaTime)
        {
            _state.OnUpdate(deltaTime);
        }

        public void ChangeStateToPostSpawn()
        {
            _state = new MeleeWalkingToPlayerEnemyState(_enemy, _triggerEnterProvider);
        }

        public void Dispose()
        {
            _state.Dispose();
        }
    }
}