using Core.Controllers;
using Core.Sounds;
using Gameplay.Services;
using GameplayTvHud.Mediators;
using UnityEngine;
using VContainer;

namespace GameplayTvHud.Actions
{
    public class FuseAction : MonoBehaviour
    {
        [SerializeField] private Camera _hudCamera;
        [SerializeField] private GameObject _handleObject;
        [SerializeField] private ActionBar _actionBar;
        [SerializeField] private float _minAngle = 270;
        [SerializeField] private float _maxAngle = 200;
        [SerializeField] private float _distanceToSwitch = 250f;
        [SerializeField] private float _counterToDisconnect = 20f;
        [SerializeField] private float _reduceMultiplier = 2f;
        [SerializeField] private float _nitroMultiplier = 2f;

        private ISoundController _soundController;
        private IActionMediator _actionMediator;
        private IInputService _inputService;

        private bool _connected;
        private bool _dragging;
        private Vector3 _startDragMousePosition;
        private bool _fullyDragged;

        private Vector3? _previousPlayerPosition;
        private float _counter;

        [Inject]
        public void Construct(ISoundController soundController, IActionMediator actionMediator, IInputService inputService)
        {
            _inputService = inputService;
            _actionMediator = actionMediator;
            _soundController = soundController;
        }

        public void StartConnected()
        {
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
            var playerPosition = _actionMediator.GetPlayerPosition();

            if (!_previousPlayerPosition.HasValue)
            {
                _previousPlayerPosition = playerPosition;
                return;
            }

            var passedDistance = Vector3.Distance(playerPosition, _previousPlayerPosition.Value);
            if (passedDistance > 0f)
            {
                var timeToAdd = Time.deltaTime * (_actionMediator.PlayerHasNitro ? _nitroMultiplier : 1f);
                _counter += timeToAdd;
            }
            else
            {
                _counter -= Time.deltaTime * _reduceMultiplier;
                _counter = Mathf.Max(_counter, 0f);
            }

            if (_counter >= _counterToDisconnect)
            {
                Disconnect();
            }

            _previousPlayerPosition = playerPosition;

            _actionBar.UpdateView(_counter / _counterToDisconnect);
        }

        private void HandleInput()
        {
            if (_inputService.IsClickDown)
            {
                var ray = _hudCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider.gameObject == _handleObject.gameObject)
                    {
                        _dragging = true;
                        _startDragMousePosition = Input.mousePosition;
                    }
                }
            }
            else if (_inputService.IsClickUp)
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
            _actionBar.DeactivateAlarm();

            _actionMediator.ConnectFuse();

            _soundController.PlaySfx(SfxType.LeverUp);
        }

        private void Disconnect()
        {
            _connected = false;
            _counter = 0f;
            SetLocalRotation(_minAngle);
            _actionBar.ActivateAlarm();

            _actionMediator.DisconnectFuse();

            _soundController.PlaySfx(SfxType.LeverDown);
        }

        private void SetLocalRotation(float angle)
        {
            _handleObject.transform.localRotation = Quaternion.Euler(angle, 0f, 0f);
        }
    }
}