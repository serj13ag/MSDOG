using System;
using System.Collections.Generic;
using Constants;
using Core.Controllers;
using Gameplay.Enemies;
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
        public void Construct(IUpdateController updateController, IArenaService arenaService)
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
            var hitEnemies = DetectEnemiesInSphere();
            foreach (var enemy in hitEnemies)
            {
                Projectile.OnHit(enemy);
            }
        }

        private List<Enemy> DetectEnemiesInSphere()
        {
            var hitEnemies = new List<Enemy>();

            var hits = Physics.OverlapSphereNonAlloc(transform.position, Projectile.Size / 2f, _hitBuffer,
                Settings.LayerMasks.EnemyLayer);
            for (var i = 0; i < hits; i++)
            {
                var hitCollider = _hitBuffer[i];
                if (hitCollider.gameObject.TryGetComponentInHierarchy<Enemy>(out var enemy))
                {
                    hitEnemies.Add(enemy);
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