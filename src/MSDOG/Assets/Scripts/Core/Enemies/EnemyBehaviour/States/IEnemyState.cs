using System;

namespace Core.Enemies.EnemyBehaviour.States
{
    public interface IEnemyState : IDisposable
    {
        void Enter();
        void OnUpdate(float deltaTime);
        void Exit();
    }
}