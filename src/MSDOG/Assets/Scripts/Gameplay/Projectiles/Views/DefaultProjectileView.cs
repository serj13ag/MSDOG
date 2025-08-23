using System;
using Core.Controllers;
using Core.Models.Data;
using Gameplay.Enemies;
using Gameplay.Factories;
using UnityEngine;
using Utility;
using Utility.Extensions;
using VContainer;

namespace Gameplay.Projectiles.Views
{
    public class DefaultProjectileView : BaseProjectileView
    {
        [SerializeField] private ColliderEventProvider _colliderEventProvider;

        private IVfxFactory _vfxFactory;

        private GameObject _impactVFXPrefab;

        [Inject]
        public void Construct(IUpdateController updateController, IVfxFactory vfxFactory)
        {
            ConstructBase(updateController);

            _vfxFactory = vfxFactory;
        }

        public void Init(Projectile projectile, ProjectileData projectileData)
        {
            InitBase(projectile);

            _impactVFXPrefab = projectileData.ImpactVFXPrefab;

            _colliderEventProvider.OnTriggerEntered += OnTriggerEntered;
        }

        protected override void OnUpdated(float deltaTime)
        {
            transform.position += Projectile.ForwardDirection * (Projectile.Speed * deltaTime);
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

        private void CreateImpactVfx()
        {
            _vfxFactory.CreatProjectileImpactEffect(transform.position, _impactVFXPrefab);
        }

        protected override void Cleanup()
        {
            base.Cleanup();

            _colliderEventProvider.OnTriggerEntered -= OnTriggerEntered;
        }
    }
}