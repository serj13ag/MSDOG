using System;
using Core.Enemies;
using Data;
using Helpers;
using Services;
using Services.Gameplay;
using UnityEngine;
using UtilityComponents;
using VContainer;

namespace Core.Projectiles.Views
{
    public class DefaultProjectileView : BaseProjectileView
    {
        [SerializeField] private ColliderEventProvider _colliderEventProvider;

        private VfxFactory _vfxFactory;

        private GameObject _impactVFXPrefab;

        [Inject]
        public void Construct(UpdateService updateService, VfxFactory vfxFactory)
        {
            ConstructBase(updateService);

            _vfxFactory = vfxFactory;

            _colliderEventProvider.OnTriggerEntered += OnTriggerEntered;
        }

        public void Init(Projectile projectile, ProjectileData projectileData)
        {
            InitBase(projectile);

            _impactVFXPrefab = projectileData.ImpactVFXPrefab;
        }

        protected override void OnUpdated(float deltaTime)
        {
            transform.position += Projectile.ForwardDirection * (Projectile.Speed * deltaTime);

            if (IsOutOfArena())
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEntered(Collider other)
        {
            if (Projectile.IsPlayer)
            {
                if (other.gameObject.TryGetComponentInHierarchy<Enemy>(out var enemy))
                {
                    Projectile.OnHit(enemy);
                }
            }
            else
            {
                if (other.gameObject.TryGetComponentInHierarchy<Player>(out var player))
                {
                    Projectile.OnHit(player);
                }
            }
        }

        protected override void OnPiercesRunOut(object sender, EventArgs e)
        {
            base.OnPiercesRunOut(sender, e);

            CreateImpactVfx();
        }

        private bool IsOutOfArena()
        {
            return Math.Abs(transform.position.x) > 50f || Math.Abs(transform.position.z) > 50f;
        }

        private void CreateImpactVfx()
        {
            _vfxFactory.CreatProjectileImpactEffect(transform.position, _impactVFXPrefab);
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            _colliderEventProvider.OnTriggerEntered -= OnTriggerEntered;
        }
    }
}