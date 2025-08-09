using UnityEngine;

namespace Gameplay.Providers
{
    public class ObjectContainerProvider
    {
        public Transform ProjectileContainer { get; }
        public Transform EnemyContainer { get; }
        public Transform DeathKitContainer { get; }
        public Transform ExperiencePieceContainer { get; }

        public ObjectContainerProvider(Transform projectileContainer, Transform enemyContainer, Transform deathKitContainer,
            Transform experiencePieceContainer)
        {
            ProjectileContainer = projectileContainer;
            EnemyContainer = enemyContainer;
            DeathKitContainer = deathKitContainer;
            ExperiencePieceContainer = experiencePieceContainer;
        }
    }
}