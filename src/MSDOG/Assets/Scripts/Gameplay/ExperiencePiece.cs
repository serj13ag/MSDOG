using Core.Controllers;
using Core.Interfaces;
using UnityEngine;
using Utility;

namespace Gameplay
{
    public class ExperiencePiece : BasePooledObject, IUpdatable
    {
        [SerializeField] private ColliderEventProvider _colliderEventProvider;
        [SerializeField] private GameObject[] _views;
        [SerializeField] private float _collectionDistance = 0.1f;
        [SerializeField] private float _flySpeed = 10f;

        private IUpdateController _updateController;
        private Player _collectorPlayer;
        private int _experience;

        public void Init(Vector3 position, int experience, IUpdateController updateController)
        {
            _updateController = updateController;
            _experience = experience;

            transform.position = position;

            updateController.Register(this);
        }

        public override void OnGet()
        {
            base.OnGet();

            ActivateRandomView();

            _colliderEventProvider.OnTriggerEntered += OnTriggerEntered;
        }

        public override void OnRelease()
        {
            base.OnRelease();

            _experience = 0;
            _collectorPlayer = null;

            if (_updateController != null)
            {
                _updateController.Remove(this);
                _updateController = null;
            }

            _colliderEventProvider.OnTriggerEntered -= OnTriggerEntered;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_collectorPlayer)
            {
                return;
            }

            var vectorToPlayer = _collectorPlayer.transform.position - transform.position;
            var isCloseToPlayer = vectorToPlayer.magnitude < _collectionDistance;
            if (isCloseToPlayer)
            {
                _collectorPlayer.CollectExperience(_experience);
                Release();
            }
            else
            {
                var directionToPlayer = vectorToPlayer.normalized;
                transform.position += directionToPlayer * (deltaTime * _flySpeed);
            }
        }

        private void OnTriggerEntered(Collider obj)
        {
            if (obj.gameObject.TryGetComponentInHierarchy<Player>(out var player))
            {
                _collectorPlayer = player;
            }
        }

        private void ActivateRandomView()
        {
            var randomIndex = Random.Range(0, _views.Length);
            for (var i = 0; i < _views.Length; i++)
            {
                _views[i].SetActive(i == randomIndex);
            }
        }
    }
}