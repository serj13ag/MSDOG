using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Services.Gameplay
{
    public class InputService
    {
        private Vector2 _moveInput;
        private bool _inputLocked;

        public Vector2 MoveInput => _moveInput;

        public event EventHandler<EventArgs> OnMenuActionPerformed;

        public InputService()
        {
            var moveAction = InputSystem.actions.FindAction("Move");
            moveAction.performed += OnInputMoveActionPerformed;
            moveAction.canceled += OnInputMoveActionCanceled;

            var menuAction = InputSystem.actions.FindAction("Menu");
            menuAction.performed += OnInputMenuActionPerformed;
        }

        public void LockInput()
        {
            _moveInput = Vector2.zero;
            _inputLocked = true;
        }

        public void UnlockInput()
        {
            _inputLocked = false;
        }

        private void OnInputMoveActionPerformed(InputAction.CallbackContext context)
        {
            if (_inputLocked)
            {
                return;
            }

            _moveInput = context.ReadValue<Vector2>();
        }

        private void OnInputMoveActionCanceled(InputAction.CallbackContext context)
        {
            if (_inputLocked)
            {
                return;
            }

            _moveInput = Vector2.zero;
        }

        private void OnInputMenuActionPerformed(InputAction.CallbackContext obj)
        {
            OnMenuActionPerformed?.Invoke(this, EventArgs.Empty);
        }
    }
}