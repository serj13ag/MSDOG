using System;
using Gameplay.Controllers;
using Gameplay.Interfaces;
using Gameplay.Services;
using Utility.Pools;

namespace Gameplay.Projectiles.Views
{
    public abstract class BaseProjectileView : BasePooledObject, IUpdatable
    {
        private const float AdditionalArenaOffset = 10f;
        private IGameplayUpdateController _updateController;
        private IArenaService _arenaService;

        private Projectile _projectile;
        private bool _shouldRelease;

        protected Projectile Projectile => _projectile;

        protected void ConstructBase(IGameplayUpdateController updateController, IArenaService arenaService)
        {
            _arenaService = arenaService;
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
            var arenaHalfSize = _arenaService.HalfSize;
            return Math.Abs(transform.position.x) > arenaHalfSize.X + AdditionalArenaOffset ||
                   Math.Abs(transform.position.z) > arenaHalfSize.Y + AdditionalArenaOffset;
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