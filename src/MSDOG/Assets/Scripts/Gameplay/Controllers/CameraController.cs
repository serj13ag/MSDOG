using Core.Controllers;
using Core.Interfaces;
using UnityEngine;
using VContainer;

namespace Gameplay.Controllers
{
    public class CameraController : MonoBehaviour, ICameraController, IUpdatable
    {
        [SerializeField] private Vector3 _cameraPositionOffset;
        [SerializeField] private Vector3 _cameraRotationOffset;

        private IUpdateController _updateController;
        private Camera _camera;

        private Transform _targetTransform;

        [Inject]
        public void Construct(IUpdateController updateController)
        {
            _updateController = updateController;
        }

        public void SetFollowTarget(Transform targetTransform)
        {
            _targetTransform = targetTransform;

            _camera = Camera.main; // TODO: fix

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