using UnityEngine;

namespace Gameplay.Services
{
    public class ObjectContainerProvider
    {
        public Transform ProjectileContainer { get; }
        public Transform EnemyContainer { get; }
        public Transform DeathKitContainer { get; }

        public ObjectContainerProvider(Transform projectileContainer, Transform enemyContainer, Transform deathKitContainer)
        {
            ProjectileContainer = projectileContainer;
            EnemyContainer = enemyContainer;
            DeathKitContainer = deathKitContainer;
        }
    }
}