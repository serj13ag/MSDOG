using System;

namespace Gameplay.Enemies.EnemyBehaviour
{
    public interface IEnemyStateMachine : IDisposable
    {
        void OnUpdate(float deltaTime);
        void ChangeStateToPostSpawn();
    }
}