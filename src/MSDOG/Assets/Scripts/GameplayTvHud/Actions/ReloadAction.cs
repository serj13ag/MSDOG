using Core.Controllers;
using Core.Sounds;
using Gameplay.Controllers;
using Gameplay.Interfaces;
using Gameplay.Services;
using GameplayTvHud.Mediators;
using UnityEngine;
using VContainer;

namespace GameplayTvHud.Actions
{
    public class ReloadAction : MonoBehaviour, IUpdatable
    {
        [SerializeField] private Camera _hudCamera;
        [SerializeField] private GameObject _handleObject;
        [SerializeField] private ActionBar _actionBar;
        [SerializeField] private float _unwindSpeed = 40f;
        [SerializeField] private float _startingAngleLerp = 0.8f;
        [SerializeField] private float _maxAngle = 1080f;

        private IGameplayUpdateController _updateController;
        private ISoundController _soundController;
        private IActionMediator _actionMediator;
        private IInputService _inputService;

        private float _currentAngle;

        private bool _dragging;
        private float _currentDragAngle;

        [Inject]
        public void Construct(IGameplayUpdateController updateController, ISoundController soundController,
            IActionMediator actionMediator, IInputService inputService)
        {
            _inputService = inputService;
            _actionMediator = actionMediator;
            _soundController = soundController;
            _updateController = updateController;

            updateController.Register(this);
        }

        private void Start()
        {
            _currentAngle = Mathf.Lerp(0f, _maxAngle, _startingAngleLerp);
            _actionBar.UpdateView(_currentAngle / _maxAngle);
        }

        public void OnUpdate(float deltaTime)
        {
            HandleInput();

            if (_dragging)
            {
                UpdateDragRotation();
            }
            else if (_currentAngle > 0f)
            {
                Unwind(deltaTime);
            }
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
                        _currentDragAngle = GetMouseAngle();
                    }
                }
            }
            else if (_inputService.IsClickUp)
            {
                _dragging = false;
            }
        }

        private void Unwind(float deltaTime)
        {
            TryRotate(-deltaTime * _unwindSpeed);
        }

        private void UpdateDragRotation()
        {
            var newAngle = GetMouseAngle();
            var angleDelta = -Mathf.DeltaAngle(_currentDragAngle, newAngle);

            if (angleDelta < 0f)
            {
                return;
            }

            TryRotate(angleDelta);
            _currentDragAngle = newAngle;
        }

        private void TryRotate(float angleDelta)
        {
            var oldAngle = _currentAngle;
            var newAngle = _currentAngle + angleDelta;
            newAngle = Mathf.Clamp(newAngle, 0f, _maxAngle);

            if (Mathf.Approximately(newAngle, _currentAngle))
            {
                return;
            }

            _currentAngle = newAngle;
            var angleIsZero = Mathf.Approximately(_currentAngle, 0f);
            if (angleIsZero)
            {
                _actionBar.ActivateAlarm();

                _actionMediator.DisablePlayerAbilities();
                _soundController.PlaySfx(SfxType.NeedReload);
            }
            else
            {
                _actionBar.DeactivateAlarm();

                _actionMediator.EnablePlayerAbilities();
            }

            _handleObject.transform.Rotate(Vector3.back, oldAngle - _currentAngle);
            _actionBar.UpdateView(_currentAngle / _maxAngle);
        }

        private float GetMouseAngle()
        {
            var objectScreenPos = _hudCamera.WorldToScreenPoint(_handleObject.transform.position);
            var dir = Input.mousePosition - objectScreenPos;
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }

        private void OnDestroy()
        {
            _updateController.Remove(this);
        }
    }
}