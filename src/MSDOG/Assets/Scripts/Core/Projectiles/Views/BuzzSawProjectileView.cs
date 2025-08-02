using Core.Enemies;
using Helpers;
using Interfaces;
using Services;
using UnityEngine;
using UtilityComponents;
using VContainer;

namespace Core.Projectiles.Views
{
    public class BuzzSawProjectileView : MonoBehaviour, IUpdatable
    {
        private const float BoxHeight = 16f;
        private const float BoxWidth = 16f;

        [SerializeField] private ColliderEventProvider _colliderEventProvider;

        private UpdateService _updateService;

        private Projectile _projectile;
        private Player _player;

        [Inject]
        public void Construct(UpdateService updateService)
        {
            _updateService = updateService;

            updateService.Register(this);
            _colliderEventProvider.OnTriggerEntered += OnTriggerEntered;
        }

        public void Init(Projectile projectile, Player player)
        {
            _projectile = projectile;
            _player = player;
        }

        public void OnUpdate(float deltaTime)
        {
            transform.position += _projectile.ForwardDirection * (_projectile.Speed * deltaTime);
            CheckBounds();
        }

        private void OnTriggerEntered(Collider other)
        {
            if (other.gameObject.TryGetComponentInHierarchy<Enemy>(out var enemy))
            {
                _projectile.OnHit(enemy);
            }
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
            var forwardDirection = _projectile.ForwardDirection;
            if (currentPos.x <= leftBound && forwardDirection.x < 0)
            {
                InvertDirectionX(forwardDirection);
                transform.position = new Vector3(leftBound, currentPos.y, currentPos.z);
            }
            else if (currentPos.x >= rightBound && forwardDirection.x > 0)
            {
                InvertDirectionX(forwardDirection);
                transform.position = new Vector3(rightBound, currentPos.y, currentPos.z);
            }

            // Check Z bounds
            if (currentPos.z <= backBound && forwardDirection.z < 0)
            {
                InvertDirectionZ(forwardDirection);
                transform.position = new Vector3(currentPos.x, currentPos.y, backBound);
            }
            else if (currentPos.z >= frontBound && forwardDirection.z > 0)
            {
                InvertDirectionZ(forwardDirection);
                transform.position = new Vector3(currentPos.x, currentPos.y, frontBound);
            }
        }

        private void InvertDirectionX(Vector3 direction)
        {
            _projectile.ChangeForwardDirection(new Vector3(-direction.x, direction.y, direction.z));
        }

        private void InvertDirectionZ(Vector3 direction)
        {
            _projectile.ChangeForwardDirection(new Vector3(direction.x, direction.y, -direction.z));
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);
            _colliderEventProvider.OnTriggerEntered -= OnTriggerEntered;
        }
    }
}