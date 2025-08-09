using UnityEngine;

namespace Gameplay.Providers
{
    public interface IObjectContainerProvider
    {
        Transform ProjectileContainer { get; }
        Transform EnemyContainer { get; }
        Transform DeathKitContainer { get; }
        Transform ExperiencePieceContainer { get; }
    }
}