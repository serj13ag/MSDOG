using System;
using Core.Enemies;
using Helpers;
using Interfaces;
using Services;
using Services.Gameplay;
using UnityEngine;
using UtilityComponents;
using VContainer;

namespace Core.Projectiles.Views
{
    public class DefaultProjectileView : MonoBehaviour, IUpdatable
    {
        [SerializeField] private ColliderEventProvider _colliderEventProvider;

        private UpdateService _updateService;
        private VfxFactory _vfxFactory;

        private Projectile _projectile;

        [Inject]
        public void Construct(UpdateService updateService, VfxFactory vfxFactory)
        {
            _updateService = updateService;
            _vfxFactory = vfxFactory;

            updateService.Register(this);
            _colliderEventProvider.OnTriggerEntered += OnTriggerEntered;
        }

        public void Init(Projectile projectile)
        {
            _projectile = projectile;

            projectile.OnPiercesRunOut += OnProjectilePiercesRunOut;
        }

        public void OnUpdate(float deltaTime)
        {
            transform.position += _projectile.ForwardDirection * (_projectile.Speed * deltaTime);

            if (IsOutOfArena())
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEntered(Collider other)
        {
            if (_projectile.Type == ProjectileType.Enemy)
            {
                if (other.gameObject.TryGetComponentInHierarchy<Player>(out var player))
                {
                    _projectile.OnHit(player);
                }
            }
            else
            {
                if (other.gameObject.TryGetComponentInHierarchy<Enemy>(out var enemy))
                {
                    _projectile.OnHit(enemy);
                }
            }
        }

        private void OnProjectilePiercesRunOut(object sender, EventArgs e)
        {
            CreateImpactVfx();
            Destroy(gameObject);
        }

        private bool IsOutOfArena()
        {
            return Math.Abs(transform.position.x) > 50f || Math.Abs(transform.position.z) > 50f;
        }

        private void CreateImpactVfx()
        {
            _vfxFactory.CreatEnemyProjectileImpactEffect(transform.position, _projectile.Type);
        }

        private void OnDestroy()
        {
            _projectile.OnPiercesRunOut += OnProjectilePiercesRunOut;

            _updateService.Remove(this);
            _colliderEventProvider.OnTriggerEntered -= OnTriggerEntered;
        }
    }
}