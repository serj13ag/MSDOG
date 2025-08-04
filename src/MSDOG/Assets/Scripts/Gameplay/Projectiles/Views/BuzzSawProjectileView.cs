using Core.Controllers;
using Gameplay.Enemies;
using UnityEngine;
using Utility;
using VContainer;

namespace Gameplay.Projectiles.Views
{
    public class BuzzSawProjectileView : BaseProjectileView
    {
        private const float BoxHeight = 16f;
        private const float BoxWidth = 16f;

        [SerializeField] private ColliderEventProvider _colliderEventProvider;

        private Player _player;

        [Inject]
        public void Construct(UpdateController updateController)
        {
            ConstructBase(updateController);

            _colliderEventProvider.OnTriggerEntered += OnTriggerEntered;
        }

        public void Init(Projectile projectile, Player player)
        {
            InitBase(projectile);

            _player = player;
        }

        protected override void OnUpdated(float deltaTime)
        {
            base.OnUpdated(deltaTime);

            transform.position += Projectile.ForwardDirection * (Projectile.Speed * deltaTime);
            CheckBounds();
        }

        private void OnTriggerEntered(Collider other)
        {
            if (other.gameObject.TryGetComponentInHierarchy<Enemy>(out var enemy))
            {
                Projectile.OnHit(enemy);
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
            var forwardDirection = Projectile.ForwardDirection;
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
            Projectile.ChangeForwardDirection(new Vector3(-direction.x, direction.y, direction.z));
        }

        private void InvertDirectionZ(Vector3 direction)
        {
            Projectile.ChangeForwardDirection(new Vector3(direction.x, direction.y, -direction.z));
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            _colliderEventProvider.OnTriggerEntered -= OnTriggerEntered;
        }
    }
}