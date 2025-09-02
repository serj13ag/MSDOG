using Gameplay.Interfaces;
using Gameplay.Services;
using UnityEngine;

namespace Gameplay.Blocks
{
    public class InputMoveBlock
    {
        private readonly IMovingEntity _movingEntity;
        private readonly CharacterController _characterController;
        private readonly IInputService _inputService;
        private readonly IArenaService _arenaService;

        public bool IsMoving { get; private set; }

        public InputMoveBlock(IMovingEntity movingEntity, CharacterController characterController, IInputService inputService,
            IArenaService arenaService)
        {
            _movingEntity = movingEntity;
            _characterController = characterController;
            _inputService = inputService;
            _arenaService = arenaService;
        }

        public void OnUpdate(float deltaTime)
        {
            var moveInput = _inputService.MoveInput;
            if (moveInput == Vector2.zero || _movingEntity.CurrentMoveSpeed <= 0f)
            {
                IsMoving = false;
                return;
            }

            var moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

            var targetRotation = Quaternion.LookRotation(moveDirection);
            var newRotation = Quaternion.RotateTowards(_movingEntity.GetRotation(), targetRotation,
                _movingEntity.RotationSpeed * deltaTime);
            _movingEntity.SetRotation(newRotation);

            var move = moveDirection * (_movingEntity.CurrentMoveSpeed * deltaTime);

            var nextPosition = _movingEntity.GetPosition() + move;
            if (Mathf.Abs(nextPosition.x) > _arenaService.HalfSize.X)
            {
                move.x = 0f;
            }

            if (Mathf.Abs(nextPosition.z) > _arenaService.HalfSize.Y)
            {
                move.z = 0f;
            }

            _characterController.Move(move);

            IsMoving = true;
        }
    }
}