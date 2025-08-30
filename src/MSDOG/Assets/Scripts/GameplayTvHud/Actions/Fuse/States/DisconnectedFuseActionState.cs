using UnityEngine;
using UnityEngine.InputSystem;

namespace GameplayTvHud.Actions.Fuse.States
{
    public class DisconnectedFuseActionState : IFuseActionState
    {
        private readonly FuseActionContext _context;

        private Vector2 _startDragMousePosition;
        private bool _dragging;
        private bool _fullyDragged;

        public DisconnectedFuseActionState(FuseActionContext context)
        {
            _context = context;
        }

        public void OnUpdate(float deltaTime)
        {
            HandleInput();

            if (_dragging)
            {
                var verticalDistance = Mouse.current.position.ReadValue().y - _startDragMousePosition.y;
                var dragAmount = verticalDistance / _context.FuseAction.DistanceToSwitch;
                _fullyDragged = dragAmount >= 1f;
                var angle = Mathf.Lerp(_context.FuseAction.MinAngle, _context.FuseAction.MaxAngle, dragAmount);

                _context.FuseAction.SetHandleLocalRotation(angle);
            }
            else
            {
                _fullyDragged = false;
            }
        }

        private void HandleInput()
        {
            if (_context.InputService.IsClickDown)
            {
                var ray = _context.HUDCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out var hit) && hit.collider.gameObject == _context.HandleObject.gameObject)
                {
                    _dragging = true;
                    _startDragMousePosition = Mouse.current.position.ReadValue();
                }
            }
            else if (_context.InputService.IsClickUp)
            {
                if (_fullyDragged)
                {
                    _context.FuseAction.ChangeStateToConnected();
                }
                else
                {
                    _dragging = false;
                    _context.FuseAction.SetHandleLocalRotation(_context.FuseAction.MinAngle);
                }
            }
        }
    }
}