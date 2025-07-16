using Core.Enemies.EnemyBehaviour.States;
using UtilityComponents;

namespace Core.Enemies.EnemyBehaviour
{
    public class MeleeBehaviourStateMachine : IEnemyStateMachine
    {
        private readonly IEnemyState _state;

        public MeleeBehaviourStateMachine(Enemy enemy, ColliderEventProvider triggerEnterProvider)
        {
            _state = new MeleeWalkingToPlayerEnemyState(enemy, triggerEnterProvider);
        }

        public void OnUpdate(float deltaTime)
        {
            _state.OnUpdate(deltaTime);
        }

        public void Dispose()
        {
            _state.Dispose();
        }
    }
}