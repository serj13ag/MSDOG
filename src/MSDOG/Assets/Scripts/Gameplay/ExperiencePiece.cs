using Core.Interfaces;
using Core.Services;
using Gameplay.Helpers;
using UnityEngine;
using UtilityComponents;

namespace Gameplay
{
    public class ExperiencePiece : MonoBehaviour, IUpdatable
    {
        [SerializeField] private ColliderEventProvider _colliderEventProvider;
        [SerializeField] private GameObject[] _views;

        private UpdateService _updateService;
        private Player _player;
        private int _experience;

        public void Init(int experience, UpdateService updateService)
        {
            _updateService = updateService;
            _experience = experience;

            var randomIndex = Random.Range(0, _views.Length);
            for (var i = 0; i < _views.Length; i++)
            {
                _views[i].SetActive(i == randomIndex);
            }

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
                _player.CollectExperience(_experience);
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