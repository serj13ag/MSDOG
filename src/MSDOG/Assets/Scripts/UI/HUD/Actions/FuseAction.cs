using Core.Controllers;
using Core.Sounds;
using Gameplay.Providers;
using Gameplay.Services;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.HUD.Actions
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

        private IPlayerProvider _playerProvider;
        private ISoundController _soundController;
        private ITutorialService _tutorialService;

        private bool _connected;
        private bool _dragging;
        private Vector3 _startDragMousePosition;
        private bool _fullyDragged;

        private Vector3? _previousPlayerPosition;
        private float _counter;

        [Inject]
        public void Construct(IPlayerProvider playerProvider, ISoundController soundController, ITutorialService tutorialService)
        {
            _playerProvider = playerProvider;
            _tutorialService = tutorialService;
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
            var player = _playerProvider.Player;

            if (!_previousPlayerPosition.HasValue)
            {
                _previousPlayerPosition = player.transform.position;
                return;
            }

            var passedDistance = Vector3.Distance(player.transform.position, _previousPlayerPosition.Value);
            _previousPlayerPosition = player.transform.position;
            if (passedDistance > 0f)
            {
                var timeToAdd = Time.deltaTime * (player.HasNitro ? _nitroMultiplier : 1f);
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

            _playerProvider.Player.MovementSetActive(true);
            _soundController.PlaySfx(SfxType.LeverUp);
        }

        private void Disconnect()
        {
            _connected = false;
            _counter = 0f;
            SetLocalRotation(_minAngle);
            _alarmIcon.ActivateAlarm();

            _playerProvider.Player.MovementSetActive(false);
            _soundController.PlaySfx(SfxType.LeverDown);
            _tutorialService.OnFuseActionDisconnected();
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