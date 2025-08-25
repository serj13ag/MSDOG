using Gameplay.Interfaces;
using UnityEngine;
using VContainer;

namespace Gameplay.Controllers
{
    public class CameraController : MonoBehaviour, ICameraController, IUpdatable
    {
        [SerializeField] private Camera _gameplayCamera;
        [SerializeField] private Vector3 _cameraPositionOffset;
        [SerializeField] private Vector3 _cameraRotationOffset;

        private IGameplayUpdateController _updateController;

        private Transform _targetTransform;

        public Camera GameplayCamera => _gameplayCamera;

        [Inject]
        public void Construct(IGameplayUpdateController updateController)
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

            _gameplayCamera.transform.position = _targetTransform.position + _cameraPositionOffset;
            _gameplayCamera.transform.rotation = Quaternion.Euler(_cameraRotationOffset);
        }

        private void OnDestroy()
        {
            _updateController.Remove(this);
        }
    }
}