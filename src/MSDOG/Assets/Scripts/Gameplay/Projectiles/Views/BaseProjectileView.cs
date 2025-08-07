using System;
using Core.Controllers;
using Core.Interfaces;
using Utility;

namespace Gameplay.Projectiles.Views
{
    public abstract class BaseProjectileView : BasePooledObject, IUpdatable
    {
        private UpdateController _updateController;

        private Projectile _projectile;

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
        }

        public override void OnGet()
        {
            base.OnGet();

            _updateController.Register(this);
        }

        public override void OnRelease()
        {
            base.OnRelease();

            _updateController.Remove(this);

            if (_projectile != null)
            {
                _projectile.OnPiercesRunOut -= OnPiercesRunOut;
                _projectile.OnTickTimeoutRaised -= OnTickTimeoutRaised;
                _projectile.OnLifetimeEnded -= OnLifetimeEnded;
            }
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

        private void OnLifetimeEnded(object sender, EventArgs e)
        {
            Release();
        }
    }
}