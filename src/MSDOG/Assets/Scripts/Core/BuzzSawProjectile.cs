using System;
using Core.Enemies;
using DTO;
using Helpers;
using Interfaces;
using Services;
using UnityEngine;
using UtilityComponents;
using VContainer;

namespace Core
{
    public class BuzzSawProjectile : MonoBehaviour, IUpdatable
    {
        private const float BoxHeight = 16f;
        private const float BoxWidth = 16f;

        [SerializeField] private ColliderEventProvider _colliderEventProvider;

        private UpdateService _updateService;

        private Player _player;
        private bool _isPlayer;
        private Guid _id;
        private Vector3 _forwardDirection;
        private int _damage;
        private float _speed;
        private int _pierce;
        private Vector3 _velocity;

        [Inject]
        public void Construct(UpdateService updateService)
        {
            _updateService = updateService;
            updateService.Register(this);
        }

        public void Init(CreateProjectileDto createProjectileDto, bool isPlayer)
        {
            _isPlayer = isPlayer;
            _id = Guid.NewGuid();
            _player = createProjectileDto.Player;
            _pierce = createProjectileDto.AbilityData.Pierce;
            _speed = createProjectileDto.AbilityData.Speed;
            _damage = createProjectileDto.AbilityData.Damage;

            _velocity = createProjectileDto.ForwardDirection * _speed;
            _velocity.y = 0f;

            _colliderEventProvider.OnTriggerEntered += OnTriggerEntered;
        }

        public void OnUpdate(float deltaTime)
        {
            transform.position += _velocity * deltaTime;
            CheckBounds();
        }

        private void CheckBounds()
        {
            var playerPos = _player.transform.position;
            var currentPos = transform.position;

            var leftBound = playerPos.x - BoxWidth / 2f;
            var rightBound = playerPos.x + BoxWidth / 2f;
            var backBound = playerPos.z - BoxHeight / 2f;
            var frontBound = playerPos.z + BoxHeight / 2f;

            // Check X bounds
            if (currentPos.x <= leftBound && _velocity.x < 0)
            {
                _velocity.x = -_velocity.x;
                transform.position = new Vector3(leftBound, currentPos.y, currentPos.z);
            }
            else if (currentPos.x >= rightBound && _velocity.x > 0)
            {
                _velocity.x = -_velocity.x;
                transform.position = new Vector3(rightBound, currentPos.y, currentPos.z);
            }

            // Check Z bounds
            if (currentPos.z <= backBound && _velocity.z < 0)
            {
                _velocity.z = -_velocity.z;
                transform.position = new Vector3(currentPos.x, currentPos.y, backBound);
            }
            else if (currentPos.z >= frontBound && _velocity.z > 0)
            {
                _velocity.z = -_velocity.z;
                transform.position = new Vector3(currentPos.x, currentPos.y, frontBound);
            }
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