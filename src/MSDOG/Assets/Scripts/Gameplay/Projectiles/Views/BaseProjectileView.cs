using System;
using Core.Controllers;
using Core.Interfaces;
using Utility;

namespace Gameplay.Projectiles.Views
{
    public abstract class BaseProjectileView : BasePooledObject, IUpdatable
    {
        private IUpdateController _updateController;

        private Projectile _projectile;
        private bool _shouldRelease;

        protected Projectile Projectile => _projectile;

        protected void ConstructBase(IUpdateController updateController)
        {
            _updateController = updateController;
        }

        protected void InitBase(Projectile projectile)
        {
            _shouldRelease = false;
            _projectile = projectile;

            _updateController.Register(this);

            projectile.OnPiercesRunOut += OnPiercesRunOut;
            projectile.OnTickTimeoutRaised += OnTickTimeoutRaised;
            projectile.OnLifetimeEnded += OnLifetimeEnded;
        }

        public void OnUpdate(float deltaTime)
        {
            _projectile.OnUpdate(deltaTime);
            OnUpdated(deltaTime);

            if (IsOutOfArena())
            {
                _shouldRelease = true;
            }

            if (_shouldRelease)
            {
                Release();
            }
        }

        protected virtual void OnUpdated(float deltaTime)
        {
        }

        protected virtual void OnPiercesRunOut(object sender, EventArgs e)
        {
            _shouldRelease = true;
        }

        protected virtual void OnTickTimeoutRaised(object sender, EventArgs e)
        {
        }

        private void OnLifetimeEnded(object sender, EventArgs e)
        {
            _shouldRelease = true;
        }

        private bool IsOutOfArena()
        {
            return Math.Abs(transform.position.x) > 50f || Math.Abs(transform.position.z) > 50f; // TODO: remove hardcode
        }

        protected override void Cleanup()
        {
            base.Cleanup();

            _updateController.Remove(this);

            _projectile?.Dispose();
            _projectile = null;
        }
    }
}