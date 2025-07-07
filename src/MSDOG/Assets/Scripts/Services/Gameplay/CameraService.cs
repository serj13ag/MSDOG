using Interfaces;
using UnityEngine;

namespace Services.Gameplay
{
    public class CameraService : MonoBehaviour, IUpdatable
    {
        [SerializeField] private Vector3 _cameraPositionOffset;
        [SerializeField] private Vector3 _cameraRotationOffset;

        private UpdateService _updateService;
        private Camera _camera;

        private Transform _targetTransform;

        public void Init(UpdateService updateService)
        {
            _updateService = updateService;

            _camera = Camera.main;

            updateService.Register(this);
        }

        public void SetFollowTarget(Transform targetTransform)
        {
            _targetTransform = targetTransform;
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