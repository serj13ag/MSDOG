using UnityEngine;

namespace GameplayTvHud.Actions.Reload.States
{
    public class IdleReloadActionState : IReloadActionState
    {
        private readonly ReloadActionContext _context;

        public IdleReloadActionState(ReloadActionContext context)
        {
            _context = context;
        }

        public void OnUpdate(float deltaTime)
        {
            if (ClickedOnHandle())
            {
                _context.ReloadAction.ChangeStateToDragging();
                return;
            }

            if (_context.ReloadAction.CurrentAngle > 0f)
            {
                _context.ReloadAction.Unwind(deltaTime);
            }
        }

        private bool ClickedOnHandle()
        {
            if (!_context.InputService.IsClickDown)
            {
                return false;
            }

            var ray = _context.HUDCamera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out var hit) && hit.collider.gameObject == _context.HandleObject.gameObject;
        }
    }
}