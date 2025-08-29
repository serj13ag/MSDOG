using Core.Controllers;
using Core.Sounds;
using Gameplay.Services;
using GameplayTvHud.Actions.Fuse.States;
using GameplayTvHud.Mediators;
using UnityEngine;
using VContainer;

namespace GameplayTvHud.Actions.Fuse
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

        private FuseActionContext _context;
        private IFuseActionState _state;

        public float MinAngle => _minAngle;
        public float MaxAngle => _maxAngle;
        public float DistanceToSwitch => _distanceToSwitch;
        public float CounterToDisconnect => _counterToDisconnect;
        public float ReduceMultiplier => _reduceMultiplier;
        public float NitroMultiplier => _nitroMultiplier;

        [Inject]
        public void Construct(ISoundController soundController, IActionMediator actionMediator, IInputService inputService)
        {
            _inputService = inputService;
            _actionMediator = actionMediator;
            _soundController = soundController;

            _context = new FuseActionContext(this, _actionMediator, _inputService, _actionBar, _hudCamera, _handleObject);
        }

        public void StartConnected()
        {
            SetHandleLocalRotation(MaxAngle);

            _actionMediator.ConnectFuse();

            _state = new ConnectedFuseActionState(_context);
        }

        private void Update()
        {
            _state?.OnUpdate(Time.deltaTime); // TODO: register in controller
        }

        public void ChangeStateToConnected()
        {
            SetHandleLocalRotation(MaxAngle);

            _actionBar.DeactivateAlarm();
            _actionMediator.ConnectFuse();
            _soundController.PlaySfx(SfxType.LeverUp);

            _state = new ConnectedFuseActionState(_context);
        }

        public void ChangeStateToDisconnected()
        {
            SetHandleLocalRotation(MinAngle);

            _actionBar.ActivateAlarm();
            _actionMediator.DisconnectFuse();
            _soundController.PlaySfx(SfxType.LeverDown);

            _state = new DisconnectedFuseActionState(_context);
        }

        public void SetHandleLocalRotation(float angle)
        {
            _handleObject.transform.localRotation = Quaternion.Euler(angle, 0f, 0f);
        }
    }
}