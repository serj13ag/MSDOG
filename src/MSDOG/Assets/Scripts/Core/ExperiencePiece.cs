using Helpers;
using Interfaces;
using Services;
using UnityEngine;
using UtilityComponents;

namespace Core
{
    public class ExperiencePiece : MonoBehaviour, IUpdatable
    {
        private const int Experience = 10;

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
                _player.CollectExperience(Experience);
                Destroy(gameObject);
                return;
            }

            var directionToPlayer = vectorToPlayer.normalized;
            transform.position += directionToPlayer * (deltaTime * 10f);
        }

        private void OnTriggerEntered(Collider obj)
        {
            if (obj.gameObject.TryGetComponentInHierarchy<Player>(out var player))
            {
                _player = player;
            }
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);
            _colliderEventProvider.OnTriggerEntered -= OnTriggerEntered;
        }
    }
}