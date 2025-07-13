using System;

namespace Core.Enemies.EnemyBehaviour.States
{
    public interface IEnemyState : IDisposable
    {
        void OnUpdate(float deltaTime);
    }
}