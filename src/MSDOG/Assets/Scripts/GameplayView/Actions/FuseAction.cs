using Core.Controllers;
using Core.Sounds;
using GameplayView.Mediators;
using UI.HUD;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GameplayView.Actions
{
    public class FuseAction : MonoBehaviour
    {
        [SerializeField] private Camera _hudCamera;
        [SerializeField] private GameObject _handleObject;
        [SerializeField] private Image _fillImage;
        [SerializeField] private AlarmIcon _alarmIcon;
        [SerializeField] private float _minAngle = 270;
        [SerializeField] private float _maxAngle = 200;
        [SerializeField] private float _distanceToSwitch = 250f;
        [SerializeField] private float _counterToDisconnect = 20f;
        [SerializeField] private float _reduceMultiplier = 2f;
        [SerializeField] private float _nitroMultiplier = 2f;

        private ISoundController _soundController;
        private IActionMediator _actionMediator;

        private bool _connected;
        private bool _dragging;
        private Vector3 _startDragMousePosition;
        private bool _fullyDragged;

        private Vector3? _previousPlayerPosition;
        private float _counter;

        [Inject]
        public void Construct(ISoundController soundController, IActionMediator actionMediator)
        {
            _actionMediator = actionMediator;
            _soundController = soundController;
        }

        public void Init()
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

            UpdateFillImageView();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
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
            _alarmIcon.DeactivateAlarm();

            _actionMediator.ConnectFuse();

            _soundController.PlaySfx(SfxType.LeverUp);
        }

        private void Disconnect()
        {
            _connected = false;
            _counter = 0f;
            SetLocalRotation(_minAngle);
            _alarmIcon.ActivateAlarm();

            _actionMediator.DisconnectFuse();

            _soundController.PlaySfx(SfxType.LeverDown);
        }

        private void SetLocalRotation(float angle)
        {
            _handleObject.transform.localRotation = Quaternion.Euler(angle, 0f, 0f);
        }

        private void UpdateFillImageView()
        {
            var t = _counter / _counterToDisconnect;
            var color = t switch
            {
                < 0.4f => Color.green,
                < 0.8f => Color.yellow,
                _ => Color.red,
            };
            _fillImage.fillAmount = t;
            _fillImage.color = color;
        }
    }
}