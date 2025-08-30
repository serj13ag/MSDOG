using System;
using Core.Controllers;
using Core.Sounds;
using Gameplay.Controllers;
using Gameplay.Interfaces;
using Gameplay.Services;
using GameplayTvHud.Actions.Fuse.States;
using GameplayTvHud.Mediators;
using UnityEngine;
using VContainer;

namespace GameplayTvHud.Actions.Fuse
{
    public class FuseAction : MonoBehaviour, IUpdatable, IDisposable
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
        private IGameplayUpdateController _gameplayUpdateController;

        private FuseActionContext _context;
        private IFuseActionState _state;

        public float MinAngle => _minAngle;
        public float MaxAngle => _maxAngle;
        public float DistanceToSwitch => _distanceToSwitch;
        public float CounterToDisconnect => _counterToDisconnect;
        public float ReduceMultiplier => _reduceMultiplier;
        public float NitroMultiplier => _nitroMultiplier;

        [Inject]
        public void Construct(ISoundController soundController, IActionMediator actionMediator, IInputService inputService,
            IGameplayUpdateController gameplayUpdateController)
        {
            _gameplayUpdateController = gameplayUpdateController;
            _actionMediator = actionMediator;
            _soundController = soundController;

            _context = new FuseActionContext(this, actionMediator, inputService, _actionBar, _hudCamera, _handleObject);

            gameplayUpdateController.Register(this);
        }

        public void StartConnected()
        {
            SetHandleLocalRotation(MaxAngle);

            _actionMediator.ConnectFuse();

            _state = new ConnectedFuseActionState(_context);
        }

        public void OnUpdate(float deltaTime)
        {
            _state?.OnUpdate(deltaTime);
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

        public void Dispose()
        {
            _gameplayUpdateController.Remove(this);
        }
    }
}