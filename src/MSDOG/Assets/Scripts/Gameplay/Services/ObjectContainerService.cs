using UnityEngine;

namespace Gameplay.Services
{
    public class ObjectContainerService
    {
        public Transform ProjectileContainer { get; }
        public Transform EnemyContainer { get; }

        public ObjectContainerService(Transform projectileContainer, Transform enemyContainer)
        {
            ProjectileContainer = projectileContainer;
            EnemyContainer = enemyContainer;
        }
    }
}