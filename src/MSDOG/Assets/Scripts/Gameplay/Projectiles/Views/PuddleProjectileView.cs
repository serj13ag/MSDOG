using System;
using System.Collections.Generic;
using Common;
using Gameplay.Controllers;
using Gameplay.Interfaces;
using Gameplay.Services;
using UnityEngine;
using Utility.Extensions;
using VContainer;

namespace Gameplay.Projectiles.Views
{
    public class PuddleProjectileView : BaseProjectileView
    {
        private const float TimeToScale = 0.5f;

        private readonly Collider[] _hitBuffer = new Collider[32];
        private float _scaleTime;

        [Inject]
        public void Construct(IGameplayUpdateController updateController, IArenaService arenaService)
        {
            ConstructBase(updateController, arenaService);
        }

        public void Init(Projectile projectile)
        {
            InitBase(projectile);

            SetLocalScale(0f);
        }

        protected override void OnUpdated(float deltaTime)
        {
            _scaleTime += deltaTime;
            if (_scaleTime < TimeToScale)
            {
                var t = _scaleTime / TimeToScale;
                var size = Mathf.Lerp(0f, Projectile.Size, t);
                SetLocalScale(size);
            }
            else
            {
                SetLocalScale(Projectile.Size);
            }
        }

        protected override void OnTickTimeoutRaised(object sender, EventArgs e)
        {
            base.OnTickTimeoutRaised(sender, e);

            Damage();
        }

        private void Damage()
        {
            var enemies = DetectEnemiesInSphere();
            foreach (var enemy in enemies)
            {
                Projectile.OnHit(enemy);
            }
        }

        private List<IProjectileDamageableEntity> DetectEnemiesInSphere()
        {
            var hitEnemies = new List<IProjectileDamageableEntity>();

            var hits = Physics.OverlapSphereNonAlloc(transform.position, Projectile.Size / 2f, _hitBuffer,
                Constants.LayerMasks.EnemyLayer);
            for (var i = 0; i < hits; i++)
            {
                var hitCollider = _hitBuffer[i];
                if (hitCollider.gameObject.TryGetComponentInHierarchy<IProjectileDamageableEntity>(out var damageable))
                {
                    hitEnemies.Add(damageable);
                }
            }

            return hitEnemies;
        }

        private void SetLocalScale(float scale)
        {
            transform.localScale = new Vector3(scale, 0.5f, scale);
        }
    }
}