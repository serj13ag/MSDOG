using UnityEngine;

namespace Core.Enemies.EnemyBehaviour
{
    public interface IEnemyStateMachine
    {
        void OnUpdate(float deltaTime);
        void OnTriggerEntered(Collider collider);
    }
}