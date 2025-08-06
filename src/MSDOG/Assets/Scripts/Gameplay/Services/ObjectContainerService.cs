using UnityEngine;

namespace Gameplay.Services
{
    public class ObjectContainerService
    {
        public Transform ProjectileContainer { get; }
        public Transform EnemyContainer { get; }
        public Transform DeathKitContainer { get; }

        public ObjectContainerService(Transform projectileContainer, Transform enemyContainer, Transform deathKitContainer)
        {
            ProjectileContainer = projectileContainer;
            EnemyContainer = enemyContainer;
            DeathKitContainer = deathKitContainer;
        }
    }
}