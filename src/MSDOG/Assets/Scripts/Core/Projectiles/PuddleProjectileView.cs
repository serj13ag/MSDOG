using System;
using System.Collections.Generic;
using Constants;
using Core.Enemies;
using Helpers;
using Interfaces;
using Services;
using UnityEngine;
using VContainer;

namespace Core.Projectiles
{
    public class PuddleProjectileView : MonoBehaviour, IUpdatable
    {
        private const float TimeToScale = 0.5f;

        private UpdateService _updateService;

        private readonly Collider[] _hitBuffer = new Collider[32];
        private Projectile _projectile;
        private float _scaleTime;

        [Inject]
        public void Construct(UpdateService updateService)
        {
            _updateService = updateService;
            updateService.Register(this);
        }

        public void Init(Projectile projectile)
        {
            _projectile = projectile;

            projectile.OnTickTimeoutRaised += OnProjectileTickTimeoutRaised;
            projectile.OnLifetimeEnded += OnProjectileLifetimeEnded;

            SetLocalScale(0f);
        }

        public void OnUpdate(float deltaTime)
        {
            _projectile.OnUpdate(deltaTime);

            _scaleTime += deltaTime;
            if (_scaleTime < TimeToScale)
            {
                var t = _scaleTime / TimeToScale;
                var size = Mathf.Lerp(0f, _projectile.Size, t);
                SetLocalScale(size);
            }
            else
            {
                SetLocalScale(_projectile.Size);
            }
        }

        private void OnProjectileLifetimeEnded(object sender, EventArgs e)
        {
            Destroy(gameObject);
        }

        private void OnProjectileTickTimeoutRaised(object sender, EventArgs e)
        {
            Damage();
        }

        private void Damage()
        {
            var hitEnemies = DetectEnemiesInSphere();
            foreach (var enemy in hitEnemies)
            {
                _projectile.OnHit(enemy);
            }
        }

        private List<Enemy> DetectEnemiesInSphere()
        {
            var hitEnemies = new List<Enemy>();

            var hits = Physics.OverlapSphereNonAlloc(transform.position, _projectile.Size / 2f, _hitBuffer,
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

        private void OnDestroy()
        {
            _updateService.Remove(this);

            _projectile.OnTickTimeoutRaised += OnProjectileTickTimeoutRaised;
            _projectile.OnLifetimeEnded += OnProjectileLifetimeEnded;
        }
    }
}