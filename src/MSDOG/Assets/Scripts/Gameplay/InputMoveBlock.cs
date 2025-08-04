using Gameplay.Services;
using UnityEngine;

namespace Gameplay
{
    public class InputMoveBlock
    {
        private readonly Player _player;
        private readonly CharacterController _characterController;
        private readonly InputService _inputService;
        private readonly ArenaService _arenaService;

        public InputMoveBlock(Player player, InputService inputService,
            ArenaService arenaService)
        {
            _player = player;
            _characterController = player.CharacterController;
            _inputService = inputService;
            _arenaService = arenaService;
        }

        public void OnUpdate(float deltaTime)
        {
            var moveInput = _inputService.MoveInput;
            if (moveInput == Vector2.zero || _player.CurrentMoveSpeed <= 0f)
            {
                _player.AnimationBlock.SetRunning(false);
                return;
            }

            var moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

            var targetRotation = Quaternion.LookRotation(moveDirection);
            _player.transform.rotation = Quaternion.RotateTowards(_player.transform.rotation, targetRotation,
                _player.RotationSpeed * deltaTime);

            var move = moveDirection * (_player.CurrentMoveSpeed * deltaTime);

            var nextPosition = _player.transform.position + move;
            if (Mathf.Abs(nextPosition.x) > _arenaService.HalfSize.X)
            {
                move.x = 0f;
            }

            if (Mathf.Abs(nextPosition.z) > _arenaService.HalfSize.Y)
            {
                move.z = 0f;
            }

            _characterController.Move(move);

            _player.AnimationBlock.SetRunning(true);
        }
    }
}