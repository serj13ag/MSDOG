using System;
using Core.Enemies;
using DTO;
using Interfaces;
using Services;
using UnityEngine;
using UtilityComponents;

namespace Core
{
    public class Projectile : MonoBehaviour, IUpdatable
    {
        [SerializeField] private ColliderEventProvider _colliderEventProvider;

        private UpdateService _updateService;

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
                var enemy = other.gameObject.GetComponentInParent<Enemy>();
                if (!enemy)
                {
                    return;
                }

                enemy.TakeDamage(_damage);
            }
            else
            {
                var player = other.gameObject.GetComponentInParent<Player>();
                if (!player)
                {
                    return;
                }

                player.RegisterProjectileDamager(_id, _damage);
            }

            if (_pierce > 0)
            {
                _pierce -= 1;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);
            _colliderEventProvider.OnTriggerEntered -= OnTriggerEntered;
        }
    }
}