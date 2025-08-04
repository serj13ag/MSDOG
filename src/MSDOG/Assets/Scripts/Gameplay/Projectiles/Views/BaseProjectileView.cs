using System;
using Core.Controllers;
using Core.Interfaces;
using UnityEngine;

namespace Gameplay.Projectiles.Views
{
    public abstract class BaseProjectileView : MonoBehaviour, IUpdatable
    {
        private UpdateController _updateController;

        private Projectile _projectile;

        protected Projectile Projectile => _projectile;

        protected void ConstructBase(UpdateController updateController)
        {
            _updateController = updateController;
            updateController.Register(this);
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
            _updateController.Remove(this);

            _projectile.OnPiercesRunOut -= OnPiercesRunOut;
            _projectile.OnTickTimeoutRaised -= OnTickTimeoutRaised;
            _projectile.OnLifetimeEnded -= OnLifetimeEnded;

            OnDestroyed();
        }
    }
}