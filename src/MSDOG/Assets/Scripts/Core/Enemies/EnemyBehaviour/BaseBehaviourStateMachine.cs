using Core.Enemies.EnemyBehaviour.States;

namespace Core.Enemies.EnemyBehaviour
{
    public abstract class BaseBehaviourStateMachine : IEnemyStateMachine
    {
        protected IEnemyState State { get; set; }

        public void OnUpdate(float deltaTime)
        {
            State.OnUpdate(deltaTime);
        }

        public abstract void ChangeStateToPostSpawn();

        protected void ChangeState(IEnemyState newState)
        {
            State?.Exit();
            State = newState;
            State.Enter();
        }

        public void Dispose()
        {
            State.Dispose();
        }
    }
}