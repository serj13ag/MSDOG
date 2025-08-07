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
        private Action _actionOnRelease;

        protected Projectile Projectile => _projectile;

        protected void ConstructBase(UpdateController updateController)
        {
            _updateController = updateController;
        }

        protected void InitBase(Projectile projectile)
        {
            _projectile = projectile;

            projectile.OnPiercesRunOut += OnPiercesRunOut;
            projectile.OnTickTimeoutRaised += OnTickTimeoutRaised;
            projectile.OnLifetimeEnded += OnLifetimeEnded;

            _updateController.Register(this);
        }

        public void SetActionOnRelease(Action actionOnRelease)
        {
            _actionOnRelease = actionOnRelease;
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
            Release();
        }

        protected virtual void OnTickTimeoutRaised(object sender, EventArgs e)
        {
        }

        protected virtual void OnBeforeReturnToPool()
        {
        }

        private void OnLifetimeEnded(object sender, EventArgs e)
        {
            Release();
        }

        protected void Release()
        {
            _updateController.Remove(this);

            _projectile.OnPiercesRunOut -= OnPiercesRunOut;
            _projectile.OnTickTimeoutRaised -= OnTickTimeoutRaised;
            _projectile.OnLifetimeEnded -= OnLifetimeEnded;

            OnBeforeReturnToPool();

            _actionOnRelease?.Invoke();
        }
    }
}