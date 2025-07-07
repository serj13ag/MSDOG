using Interfaces;
using Services;
using Services.Gameplay;
using UnityEngine;

namespace Core
{
    public class Player : MonoBehaviour, IUpdatable
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _moveSpeed;

        private InputService _inputService;
        private UpdateService _updateService;

        public void Init(InputService inputService, UpdateService updateService)
        {
            _updateService = updateService;
            _inputService = inputService;

            updateService.Register(this);
        }

        public void OnUpdate(float deltaTime)
        {
            var moveInput = _inputService.MoveInput;
            if (moveInput == Vector2.zero)
            {
                return;
            }

            var moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            var move = moveDirection * (_moveSpeed * deltaTime);
            _characterController.Move(move);
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);
        }
    }
}