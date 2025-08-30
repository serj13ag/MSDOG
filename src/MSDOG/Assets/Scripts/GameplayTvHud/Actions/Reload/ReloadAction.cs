using Core.Controllers;
using Core.Sounds;
using Gameplay.Controllers;
using Gameplay.Interfaces;
using Gameplay.Services;
using GameplayTvHud.Actions.Reload.States;
using GameplayTvHud.Mediators;
using UnityEngine;
using VContainer;

namespace GameplayTvHud.Actions.Reload
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

        private ReloadActionContext _context;
        private IReloadActionState _state;

        private float _currentAngle;

        public float CurrentAngle => _currentAngle;

        [Inject]
        public void Construct(IGameplayUpdateController updateController, ISoundController soundController,
            IActionMediator actionMediator, IInputService inputService)
        {
            _actionMediator = actionMediator;
            _soundController = soundController;
            _updateController = updateController;

            _context = new ReloadActionContext(this, inputService, _hudCamera, _handleObject);
            ChangeStateToIdle();

            updateController.Register(this);
        }

        private void Start()
        {
            _currentAngle = Mathf.Lerp(0f, _maxAngle, _startingAngleLerp);
            UpdateActionBar();
        }

        public void OnUpdate(float deltaTime)
        {
            _state?.OnUpdate(deltaTime);
        }

        public void ChangeStateToDragging()
        {
            _state = new DraggingReloadActionState(_context);
        }

        public void ChangeStateToIdle()
        {
            _state = new IdleReloadActionState(_context);
        }

        public void Unwind(float deltaTime)
        {
            TryRotate(-deltaTime * _unwindSpeed);
        }

        public void TryRotate(float angleDelta)
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
            UpdateActionBar();
        }

        private void UpdateActionBar()
        {
            _actionBar.UpdateView(_currentAngle / _maxAngle);
        }

        private void OnDestroy()
        {
            _updateController.Remove(this);
        }
    }
}