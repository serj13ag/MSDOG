using Core.Controllers;
using Core.Interfaces;
using UnityEngine;
using Utility;
using Utility.Extensions;
using Utility.Pools;
using VContainer;

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

        [Inject]
        public void Construct(IUpdateController updateController)
        {
            _updateController = updateController;
        }

        public void Init(Vector3 position, int experience)
        {
            _experience = experience;
            _collectorPlayer = null;

            ActivateRandomView();

            transform.position = position;

            _updateController.Register(this);

            _colliderEventProvider.OnTriggerEntered += OnTriggerEntered;
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

        protected override void Cleanup()
        {
            base.Cleanup();

            _updateController?.Remove(this);

            _colliderEventProvider.OnTriggerEntered -= OnTriggerEntered;
        }
    }
}