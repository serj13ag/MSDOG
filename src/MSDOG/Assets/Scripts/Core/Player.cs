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
        private ArenaService _arenaService;

        public void Init(InputService inputService, UpdateService updateService, ArenaService arenaService)
        {
            _arenaService = arenaService;
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

            var nextPosition = transform.position + move;
            if (Mathf.Abs(nextPosition.x) > _arenaService.HalfSize.X)
            {
                move.x = 0f;
            }

            if (Mathf.Abs(nextPosition.z) > _arenaService.HalfSize.Y)
            {
                move.z = 0f;
            }

            _characterController.Move(move);
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);
        }
    }
}