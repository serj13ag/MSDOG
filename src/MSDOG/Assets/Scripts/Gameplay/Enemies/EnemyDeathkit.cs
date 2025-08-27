using DG.Tweening;
using Gameplay.Controllers;
using Gameplay.Interfaces;
using UnityEngine;
using Utility.Pools;
using VContainer;
using Random = UnityEngine.Random;

namespace Gameplay.Enemies
{
    public class EnemyDeathkit : BasePooledObject, IUpdatable
    {
        [SerializeField] private GameObject[] _parts;
        [SerializeField] private Transform _forceCenter;
        [SerializeField] private float _force;
        [SerializeField] private float _forceRadius;
        [SerializeField] private float _hideCooldown = 5f;
        [SerializeField] private float _torqueMultiplier = 10f;

        private IGameplayUpdateController _gameplayUpdateController;

        private Vector3[] _partInitialLocalPositions;
        private Quaternion[] _partInitialLocalRotations;
        private Rigidbody[] _partRigidbodies;
        private Sequence _sequence;

        [Inject]
        public void Construct(IGameplayUpdateController gameplayUpdateController)
        {
            _gameplayUpdateController = gameplayUpdateController;
        }

        private void Awake()
        {
            _partInitialLocalPositions = new Vector3[_parts.Length];
            _partInitialLocalRotations = new Quaternion[_parts.Length];
            _partRigidbodies = new Rigidbody[_parts.Length];

            for (var i = 0; i < _parts.Length; i++)
            {
                var part = _parts[i];

                _partInitialLocalPositions[i] = part.transform.localPosition;
                _partInitialLocalRotations[i] = part.transform.localRotation;

                var rb = part.gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                _partRigidbodies[i] = rb;

                var meshCollider = part.gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true;
            }
        }

        public override void OnGet()
        {
            base.OnGet();

            _gameplayUpdateController.Register(this);
        }

        public void Init(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;

            for (var i = 0; i < _parts.Length; i++)
            {
                var rb = _partRigidbodies[i];
                rb.isKinematic = false;
                rb.AddExplosionForce(_force, _forceCenter.transform.position, _forceRadius);

                var randomTorque = new Vector3(
                    Random.Range(-_torqueMultiplier, _torqueMultiplier),
                    Random.Range(-_torqueMultiplier, _torqueMultiplier),
                    Random.Range(-_torqueMultiplier, _torqueMultiplier)
                );
                rb.AddTorque(randomTorque);
            }

            _sequence = DOTween.Sequence()
                .InsertCallback(_hideCooldown, () =>
                {
                    foreach (var part in _parts)
                    {
                        part.GetComponent<Rigidbody>().isKinematic = true;
                    }
                })
                .Insert(_hideCooldown, transform.DOMoveY(-2f, 5f))
                .SetUpdate(UpdateType.Manual)
                .OnComplete(Release);
        }

        public void OnUpdate(float deltaTime)
        {
            _sequence?.ManualUpdate(deltaTime, deltaTime);
        }

        protected override void Cleanup()
        {
            base.Cleanup();

            for (var i = 0; i < _parts.Length; i++)
            {
                var part = _parts[i];
                part.transform.localPosition = _partInitialLocalPositions[i];
                part.transform.localRotation = _partInitialLocalRotations[i];
            }

            _sequence?.Kill();

            _gameplayUpdateController.Remove(this);
        }
    }
}