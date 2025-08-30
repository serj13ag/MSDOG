using UnityEngine;
using UnityEngine.InputSystem;

namespace GameplayTvHud.Actions.Reload.States
{
    public class DraggingReloadActionState : IReloadActionState
    {
        private readonly ReloadActionContext _context;

        private float _currentDragAngle;

        public DraggingReloadActionState(ReloadActionContext context)
        {
            _context = context;

            _currentDragAngle = GetMouseAngle();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_context.InputService.IsClickUp)
            {
                _context.ReloadAction.ChangeStateToIdle();
                return;
            }

            UpdateDragRotation();
        }

        private void UpdateDragRotation()
        {
            var newAngle = GetMouseAngle();
            var angleDelta = -Mathf.DeltaAngle(_currentDragAngle, newAngle);

            if (angleDelta < 0f)
            {
                return;
            }

            _context.ReloadAction.TryRotate(angleDelta);
            _currentDragAngle = newAngle;
        }

        private float GetMouseAngle()
        {
            var objectScreenPos = _context.HUDCamera.WorldToScreenPoint(_context.HandleObject.transform.position);
            var dir = (Vector3)Mouse.current.position.ReadValue() - objectScreenPos;
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
    }
}