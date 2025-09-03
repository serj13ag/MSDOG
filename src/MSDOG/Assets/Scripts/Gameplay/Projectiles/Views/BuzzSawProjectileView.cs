using Gameplay.Controllers;
using Gameplay.Interfaces;
using Gameplay.Services;
using UnityEngine;
using Utility;
using Utility.Extensions;
using VContainer;

namespace Gameplay.Projectiles.Views
{
    public class BuzzSawProjectileView : BaseProjectileView
    {
        private const float BoxHeight = 16f;
        private const float BoxWidth = 16f;

        [SerializeField] private ColliderEventProvider _colliderEventProvider;

        private IEntityWithPosition _centeredEntity;

        [Inject]
        public void Construct(IGameplayUpdateController updateController, IArenaService arenaService)
        {
            ConstructBase(updateController, arenaService);
        }

        public void Init(Projectile projectile, IEntityWithPosition centeredEntity)
        {
            InitBase(projectile);

            _centeredEntity = centeredEntity;

            _colliderEventProvider.OnTriggerEntered += OnTriggerEntered;
        }

        protected override void OnUpdated(float deltaTime)
        {
            base.OnUpdated(deltaTime);

            transform.position += Projectile.ForwardDirection * (Projectile.Speed * deltaTime);
            CheckBounds();
        }

        private void OnTriggerEntered(Collider other)
        {
            if (other.gameObject.TryGetComponentInHierarchy<IProjectileDamageableEntity>(out var projectileDamageableEntity))
            {
                Projectile.OnHit(projectileDamageableEntity);
            }
        }

        private void CheckBounds()
        {
            var playerPos = _centeredEntity.GetPosition();
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

        protected override void Cleanup()
        {
            base.Cleanup();

            _colliderEventProvider.OnTriggerEntered -= OnTriggerEntered;
        }
    }
}