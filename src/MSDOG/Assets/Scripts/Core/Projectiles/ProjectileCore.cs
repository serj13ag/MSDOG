using System;
using Core.Enemies;
using DTO;
using UnityEngine;

namespace Core.Projectiles
{
    public class ProjectileCore
    {
        private readonly Guid _id;
        private readonly int _damage;
        private int _pierce;

        public ProjectileType Type { get; }
        public Vector3 ForwardDirection { get; }
        public float Speed { get; }

        public event EventHandler<EventArgs> OnDestroyed;

        public ProjectileCore(CreateProjectileDto createProjectileDto, ProjectileType projectileType)
        {
            _id = Guid.NewGuid();
            _damage = createProjectileDto.Damage;
            _pierce = createProjectileDto.Pierce;

            Type = projectileType;
            ForwardDirection = createProjectileDto.ForwardDirection.normalized;
            Speed = createProjectileDto.Speed;
        }

        public void OnHit(Player player)
        {
            player.RegisterProjectileDamager(_id, _damage);
            CheckPierce();
        }

        public void OnHit(Enemy enemy)
        {
            enemy.TakeDamage(_damage);
            CheckPierce();
        }

        private void CheckPierce()
        {
            if (_pierce > 0)
            {
                _pierce -= 1;
            }
            else
            {
                OnDestroyed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}