using UnityEngine;
using UnityEngine.InputSystem;

namespace Services.Gameplay
{
    public class InputService
    {
        private readonly InputAction _moveAction;
        private Vector2 _moveInput;
        private bool _inputLocked;

        public Vector2 MoveInput => _moveInput;

        public InputService()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _moveAction.performed += OnMoveActionPerformed;
            _moveAction.canceled += OnMoveActionCanceled;
        }

        private void OnMoveActionPerformed(InputAction.CallbackContext context)
        {
            if (_inputLocked)
            {
                return;
            }

            _moveInput = context.ReadValue<Vector2>();
        }

        private void OnMoveActionCanceled(InputAction.CallbackContext context)
        {
            if (_inputLocked)
            {
                return;
            }

            _moveInput = Vector2.zero;
        }

        public void LockInput()
        {
            _moveInput = Vector2.zero;
            _inputLocked = true;
        }
    }
}