using UnityEngine;

namespace Gameplay.Providers
{
    public class ObjectContainerProvider : IObjectContainerProvider
    {
        public Transform ProjectileContainer { get; }
        public Transform EnemyContainer { get; }
        public Transform DeathKitContainer { get; }
        public Transform ExperiencePieceContainer { get; }
        public Transform DamageTextContainer { get; }

        public ObjectContainerProvider(Transform projectileContainer, Transform enemyContainer, Transform deathKitContainer,
            Transform experiencePieceContainer, Transform damageTextContainer)
        {
            ProjectileContainer = projectileContainer;
            EnemyContainer = enemyContainer;
            DeathKitContainer = deathKitContainer;
            ExperiencePieceContainer = experiencePieceContainer;
            DamageTextContainer = damageTextContainer;
        }
    }
}