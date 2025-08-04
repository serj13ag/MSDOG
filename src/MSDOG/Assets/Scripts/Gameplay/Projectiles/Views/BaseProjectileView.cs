using System;
using Core.Controllers;
using Core.Interfaces;
using UnityEngine;

namespace Gameplay.Projectiles.Views
{
    public abstract class BaseProjectileView : MonoBehaviour, IUpdatable
    {
        private UpdateService _updateService;

        private Projectile _projectile;

        protected Projectile Projectile => _projectile;

        protected void ConstructBase(UpdateService updateService)
        {
            _updateService = updateService;
            updateService.Register(this);
        }

        protected void InitBase(Projectile projectile)
        {
            _projectile = projectile;

            projectile.OnPiercesRunOut += OnPiercesRunOut;
            projectile.OnTickTimeoutRaised += OnTickTimeoutRaised;
            projectile.OnLifetimeEnded += OnLifetimeEnded;
        }

        public void OnUpdate(float deltaTime)
        {
            _projectile.OnUpdate(deltaTime);

            OnUpdated(deltaTime);
        }

        protected virtual void OnUpdated(float deltaTime)
        {
        }

        protected virtual void OnPiercesRunOut(object sender, EventArgs e)
        {
            Destroy(gameObject);
        }

        protected virtual void OnTickTimeoutRaised(object sender, EventArgs e)
        {
        }

        private void OnLifetimeEnded(object sender, EventArgs e)
        {
            Destroy(gameObject);
        }

        protected virtual void OnDestroyed()
        {
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);

            _projectile.OnPiercesRunOut -= OnPiercesRunOut;
            _projectile.OnTickTimeoutRaised -= OnTickTimeoutRaised;
            _projectile.OnLifetimeEnded -= OnLifetimeEnded;

            OnDestroyed();
        }
    }
}