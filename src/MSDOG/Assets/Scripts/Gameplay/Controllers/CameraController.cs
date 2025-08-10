using Core.Controllers;
using Core.Interfaces;
using UnityEngine;
using VContainer;

namespace Gameplay.Controllers
{
    public class CameraController : MonoBehaviour, ICameraController, IUpdatable
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Vector3 _cameraPositionOffset;
        [SerializeField] private Vector3 _cameraRotationOffset;

        private IUpdateController _updateController;

        private Transform _targetTransform;

        public Camera Camera => _camera;

        [Inject]
        public void Construct(IUpdateController updateController)
        {
            _updateController = updateController;
        }

        public void SetFollowTarget(Transform targetTransform)
        {
            _targetTransform = targetTransform;

            _updateController.Register(this);
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
            _updateController.Remove(this);
        }
    }
}