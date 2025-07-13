using System;

namespace Core.Enemies.EnemyBehaviour
{
    public interface IEnemyStateMachine : IDisposable
    {
        void OnUpdate(float deltaTime);
    }
}