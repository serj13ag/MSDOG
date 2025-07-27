using System;
using Core.Enemies;
using DTO;
using Helpers;
using Interfaces;
using Services;
using Services.Gameplay;
using UnityEngine;
using UtilityComponents;

namespace Core
{
    public class Projectile : MonoBehaviour, IUpdatable
    {
        [SerializeField] private ColliderEventProvider _colliderEventProvider;
        [SerializeField] private SpriteAnimatorComponent _impactSpriteAnimator;

        private UpdateService _updateService;
        private VfxFactory _vfxFactory;

        private bool _isPlayer;
        private Guid _id;
        private Vector3 _forwardDirection;
        private int _damage;
        private float _speed;
        private int _pierce;

        public void Init(CreateProjectileDto createProjectileDto, UpdateService updateService, bool isPlayer)
        {
            _updateService = updateService;

            _isPlayer = isPlayer;
            _id = Guid.NewGuid();
            _pierce = createProjectileDto.AbilityData.Pierce;
            _speed = createProjectileDto.AbilityData.Speed;
            _damage = createProjectileDto.AbilityData.Damage;
            _forwardDirection = createProjectileDto.ForwardDirection;

            updateService.Register(this);
            _colliderEventProvider.OnTriggerEntered += OnTriggerEntered;
        }

        public void Init(CreateEnemyProjectileDto createProjectileDto, UpdateService updateService, VfxFactory vfxFactory,
            bool isPlayer)
        {
            _vfxFactory = vfxFactory;
            _updateService = updateService;

            _isPlayer = isPlayer;
            _id = Guid.NewGuid();
            _pierce = createProjectileDto.Pierce;
            _speed = createProjectileDto.Speed;
            _damage = createProjectileDto.Damage;
            _forwardDirection = createProjectileDto.ForwardDirection;

            updateService.Register(this);
            _colliderEventProvider.OnTriggerEntered += OnTriggerEntered;
        }

        public void OnUpdate(float deltaTime)
        {
            transform.position += _forwardDirection.normalized * (_speed * deltaTime);
        }

        private void OnTriggerEntered(Collider other)
        {
            if (_isPlayer)
            {
                if (other.gameObject.TryGetComponentInHierarchy<Enemy>(out var enemy))
                {
                    enemy.TakeDamage(_damage);
                }
            }
            else
            {
                if (other.gameObject.TryGetComponentInHierarchy<Player>(out var player))
                {
                    player.RegisterProjectileDamager(_id, _damage);
                }
            }

            if (_pierce > 0)
            {
                _pierce -= 1;
            }
            else
            {
                CreateImpactVfx();
                Destroy(gameObject);
            }
        }

        private void CreateImpactVfx()
        {
            if (!_isPlayer)
            {
                _vfxFactory.CreatEnemyProjectileImpactEffect(transform.position);
            }
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);
            _colliderEventProvider.OnTriggerEntered -= OnTriggerEntered;
        }
    }
}