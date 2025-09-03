using System;
using Gameplay.Interfaces;
using UnityEngine;

namespace Gameplay.Projectiles
{
    public class Projectile : IDisposable
    {
        private readonly Guid _id;
        private readonly int _damage;
        private int? _piercesLeft;
        private Vector3 _forwardDirection;
        private float? _lifetimeRemaining;

        private readonly float? _tickTimeout;
        private float _tickRemaining;

        public bool IsPlayer { get; } // TODO: remove?
        public float Speed { get; }
        public float Size { get; }

        public Vector3 ForwardDirection
        {
            get => _forwardDirection;
            private set
            {
                var newForwardDirection = value.normalized;
                newForwardDirection.y = 0;
                _forwardDirection = newForwardDirection;
            }
        }

        public event EventHandler<EventArgs> OnPiercesRunOut;
        public event EventHandler<EventArgs> OnLifetimeEnded;
        public event EventHandler<EventArgs> OnTickTimeoutRaised;

        public Projectile(ProjectileSpawnData projectileSpawnData, bool isPlayer)
        {
            _id = Guid.NewGuid();
            _damage = projectileSpawnData.Damage;

            var pierce = projectileSpawnData.Pierce;
            _piercesLeft = pierce == -1 ? null : pierce;

            var lifetime = projectileSpawnData.Lifetime;
            _lifetimeRemaining = lifetime == 0f ? null : lifetime;

            var tickTimeout = projectileSpawnData.TickTimeout;
            _tickTimeout = tickTimeout == 0f ? null : tickTimeout;
            _tickRemaining = tickTimeout;

            IsPlayer = isPlayer;
            Speed = projectileSpawnData.Speed;
            Size = projectileSpawnData.Size;
            ForwardDirection = projectileSpawnData.ForwardDirection.normalized;
        }

        public void ChangeForwardDirection(Vector3 newForwardDirection)
        {
            ForwardDirection = newForwardDirection;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_lifetimeRemaining.HasValue)
            {
                _lifetimeRemaining -= deltaTime;
                if (_lifetimeRemaining < 0f)
                {
                    OnLifetimeEnded?.Invoke(this, EventArgs.Empty);
                }
            }

            if (_tickTimeout.HasValue)
            {
                _tickRemaining -= deltaTime;
                if (_tickRemaining < 0f)
                {
                    _tickRemaining = _tickTimeout.Value;
                    OnTickTimeoutRaised?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void OnHit(IProjectileDamageableEntity projectileDamageableEntity)
        {
            projectileDamageableEntity.TakeProjectileDamage(_id, _damage);
            CheckPierce();
        }

        private void CheckPierce()
        {
            if (!_piercesLeft.HasValue)
            {
                return;
            }

            if (_piercesLeft > 0)
            {
                _piercesLeft -= 1;
            }
            else
            {
                OnPiercesRunOut?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            OnPiercesRunOut = null;
            OnLifetimeEnded = null;
            OnTickTimeoutRaised = null;
        }
    }
}