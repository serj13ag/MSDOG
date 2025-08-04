using Core.Controllers;
using Core.Interfaces;
using UnityEngine;
using VContainer;

namespace Gameplay.Controllers
{
    public class CameraService : MonoBehaviour, IUpdatable
    {
        [SerializeField] private Vector3 _cameraPositionOffset;
        [SerializeField] private Vector3 _cameraRotationOffset;

        private UpdateService _updateService;
        private Camera _camera;

        private Transform _targetTransform;

        [Inject]
        public void Construct(UpdateService updateService)
        {
            _updateService = updateService;
        }

        public void SetFollowTarget(Transform targetTransform)
        {
            _targetTransform = targetTransform;

            _camera = Camera.main;

            _updateService.Register(this);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_targetTransform == null)
            {
                return;
            }

            _camera.transform.position = _targetTransform.position + _cameraPositionOffset;
            _camera.transform.rotation = Quaternion.Euler(_cameraRotationOffset);
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);
        }
    }
}