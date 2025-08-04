using UnityEngine;

namespace Gameplay.Services
{
    public class RuntimeContainers
    {
        public Transform ProjectileContainer { get; }
        public Transform EnemyContainer { get; }

        public RuntimeContainers(Transform projectileContainer, Transform enemyContainer)
        {
            ProjectileContainer = projectileContainer;
            EnemyContainer = enemyContainer;
        }
    }
}