using Core;
using UnityEngine;

namespace UI.HUD.Actions
{
    public class FuseAction : MonoBehaviour
    {
        [SerializeField] private Camera _hudCamera;
        [SerializeField] private float _minAngle = 270;
        [SerializeField] private float _maxAngle = 200;
        [SerializeField] private float _distanceToSwitch = 250f;
        [SerializeField] private float _counterToDisconnect = 3f;

        private Player _player;

        private bool _connected;
        private bool _dragging;
        private Vector3 _startDragMousePosition;
        private bool _fullyDragged;

        private Vector3? _previousPlayerPosition;
        private float _counter;

        public void Init(Player player)
        {
            _player = player;

            Connect();
        }

        private void Update()
        {
            if (_connected)
            {
                UpdateCounter();
                return;
            }

            HandleInput();

            if (_dragging)
            {
                var verticalDistance = Input.mousePosition.y - _startDragMousePosition.y;
                var dragAmount = verticalDistance / _distanceToSwitch;
                _fullyDragged = dragAmount >= 1f;
                var angle = Mathf.Lerp(_minAngle, _maxAngle, dragAmount);
                SetLocalRotation(angle);
            }
            else
            {
                _fullyDragged = false;
            }
        }

        private void UpdateCounter()
        {
            if (!_previousPlayerPosition.HasValue)
            {
                _previousPlayerPosition = _player.transform.position;
                return;
            }

            var passedDistance = Vector3.Distance(_player.transform.position, _previousPlayerPosition.Value);
            _previousPlayerPosition = _player.transform.position;
            if (passedDistance > 0f)
            {
                _counter += Time.deltaTime;
            }
            else
            {
                _counter -= Time.deltaTime;
                _counter = Mathf.Max(_counter, 0f);
            }

            if (_counter >= _counterToDisconnect)
            {
                Disconnect();
            }
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _hudCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        _dragging = true;
                        _startDragMousePosition = Input.mousePosition;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _dragging = false;
                if (_fullyDragged)
                {
                    Connect();
                }
                else
                {
                    SetLocalRotation(_minAngle);
                }
            }
        }

        private void Connect()
        {
            _connected = true;
            _previousPlayerPosition = null;
            SetLocalRotation(_maxAngle);
            
            _player.MovementSetActive(true);
        }

        private void Disconnect()
        {
            _connected = false;
            _counter = 0f;
            SetLocalRotation(_minAngle);

            _player.MovementSetActive(false);
        }

        private void SetLocalRotation(float angle)
        {
            transform.localRotation = Quaternion.Euler(angle, 0f, 0f);
        }
    }
}