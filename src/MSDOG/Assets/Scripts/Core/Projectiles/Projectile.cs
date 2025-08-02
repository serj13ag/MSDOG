using System;
using Core.Enemies;
using Helpers;
using Interfaces;
using Services;
using Services.Gameplay;
using UnityEngine;
using UtilityComponents;
using VContainer;

namespace Core.Projectiles
{
    public class Projectile : MonoBehaviour, IUpdatable
    {
        [SerializeField] private ColliderEventProvider _colliderEventProvider;

        private UpdateService _updateService;
        private VfxFactory _vfxFactory;

        private ProjectileCore _projectileCore;

        [Inject]
        public void Construct(UpdateService updateService, VfxFactory vfxFactory)
        {
            _updateService = updateService;
            _vfxFactory = vfxFactory;

            updateService.Register(this);
            _colliderEventProvider.OnTriggerEntered += OnTriggerEntered;
        }

        public void Init(ProjectileCore projectileCore)
        {
            _projectileCore = projectileCore;

            projectileCore.OnPiercesRunOut += OnProjectilePiercesRunOut;
        }

        public void OnUpdate(float deltaTime)
        {
            transform.position += _projectileCore.ForwardDirection * (_projectileCore.Speed * deltaTime);

            if (IsOutOfArena())
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEntered(Collider other)
        {
            if (_projectileCore.Type == ProjectileType.Enemy)
            {
                if (other.gameObject.TryGetComponentInHierarchy<Player>(out var player))
                {
                    _projectileCore.OnHit(player);
                }
            }
            else
            {
                if (other.gameObject.TryGetComponentInHierarchy<Enemy>(out var enemy))
                {
                    _projectileCore.OnHit(enemy);
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
            _vfxFactory.CreatEnemyProjectileImpactEffect(transform.position, _projectileCore.Type);
        }

        private void OnDestroy()
        {
            _projectileCore.OnPiercesRunOut += OnProjectilePiercesRunOut;

            _updateService.Remove(this);
            _colliderEventProvider.OnTriggerEntered -= OnTriggerEntered;
        }
    }
}