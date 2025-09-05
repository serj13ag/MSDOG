using System;
using Gameplay.Interfaces;
using UnityEngine;

namespace Gameplay.Enemies
{
    public interface IEnemy : IEntityWithPosition, IEntityWithRotation, IProjectileDamageableEntity, IHitDamageableEntity
    {
        Guid Id { get; }
        float Cooldown { get; }
        int Damage { get; }
        EnemyDeathkit DeathkitPrefab { get; }
        Vector3 ModelRootPosition { get; }

        event Action<IEnemy> OnDied;

        void Shoot();
        void Kill();
    }
}