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

        private IEntityWithPosition _targetEntityWithPosition;

        public Camera GameplayCamera => _gameplayCamera;

        [Inject]
        public void Construct(IGameplayUpdateController updateController)
        {
            _updateController = updateController;
        }

        public void SetFollowTarget(IEntityWithPosition entityWithPosition)
        {
            _targetEntityWithPosition = entityWithPosition;

            _updateController.Register(this);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_targetEntityWithPosition == null)
            {
                return;
            }

            _gameplayCamera.transform.position = _targetEntityWithPosition.GetPosition() + _cameraPositionOffset;
            _gameplayCamera.transform.rotation = Quaternion.Euler(_cameraRotationOffset);
        }

        private void OnDestroy()
        {
            _updateController.Remove(this);
        }
    }
}