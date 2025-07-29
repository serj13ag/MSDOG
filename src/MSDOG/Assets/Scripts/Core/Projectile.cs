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

        private ProjectileType _type;
        private Guid _id;
        private Vector3 _forwardDirection;
        private int _damage;
        private float _speed;
        private int _pierce;

        public void Init(CreateProjectileDto createProjectileDto, UpdateService updateService, VfxFactory vfxFactory,
            ProjectileType type)
        {
            _vfxFactory = vfxFactory;
            _updateService = updateService;

            _type = type;
            _id = Guid.NewGuid();
            _pierce = createProjectileDto.AbilityData.Pierce;
            _speed = createProjectileDto.AbilityData.Speed;
            _damage = createProjectileDto.AbilityData.Damage;
            _forwardDirection = createProjectileDto.ForwardDirection;

            updateService.Register(this);
            _colliderEventProvider.OnTriggerEntered += OnTriggerEntered;
        }

        public void Init(CreateEnemyProjectileDto createProjectileDto, UpdateService updateService, VfxFactory vfxFactory,
            ProjectileType type)
        {
            _vfxFactory = vfxFactory;
            _updateService = updateService;

            _type = type;
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

            if (Math.Abs(transform.position.x) > 50f || Math.Abs(transform.position.z) > 50f) // TODO: refactor
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEntered(Collider other)
        {
            if (_type == ProjectileType.Enemy)
            {
                if (other.gameObject.TryGetComponentInHierarchy<Player>(out var player))
                {
                    player.RegisterProjectileDamager(_id, _damage);
                }
            }
            else
            {
                if (other.gameObject.TryGetComponentInHierarchy<Enemy>(out var enemy))
                {
                    enemy.TakeDamage(_damage);
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
            _vfxFactory.CreatEnemyProjectileImpactEffect(transform.position, _type);
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);
            _colliderEventProvider.OnTriggerEntered -= OnTriggerEntered;
        }
    }
}