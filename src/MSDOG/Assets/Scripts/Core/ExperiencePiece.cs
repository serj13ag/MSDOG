using Interfaces;
using Services;
using UnityEngine;
using UtilityComponents;

namespace Core
{
    public class ExperiencePiece : MonoBehaviour, IUpdatable
    {
        [SerializeField] private ColliderEventProvider _colliderEventProvider;

        private UpdateService _updateService;
        private Player _player;

        public void Init(UpdateService updateService)
        {
            _updateService = updateService;

            updateService.Register(this);
            _colliderEventProvider.OnTriggerEntered += OnTriggerEntered;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_player)
            {
                return;
            }

            var vectorToPlayer = _player.transform.position - transform.position;
            if (vectorToPlayer.sqrMagnitude < 0.1f)
            {
                // TODO: add exp to player
                Destroy(gameObject);
                return;
            }

            var directionToPlayer = vectorToPlayer.normalized;
            transform.position += directionToPlayer * (deltaTime * 10f);
        }

        private void OnTriggerEntered(Collider obj)
        {
            var player = obj.GetComponentInParent<Player>();
            if (!player)
            {
                return;
            }

            _player = player;
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);
            _colliderEventProvider.OnTriggerEntered -= OnTriggerEntered;
        }
    }
}