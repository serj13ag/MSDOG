using Core.Controllers;
using Core.Sounds;
using Gameplay.Services;
using GameplayTvHud.Mediators;
using UnityEngine;
using VContainer;

namespace GameplayTvHud.Actions
{
    public class NitroAction : MonoBehaviour
    {
        [SerializeField] private Camera _hudCamera;
        [SerializeField] private GameObject _buttonObject;
        [SerializeField] private float _moveSpeedMultiplier = 2f;
        [SerializeField] private float _offPositionZ = 1.4f;
        [SerializeField] private float _onPositionZ = 1f;

        private ISoundController _soundController;
        private IActionMediator _actionMediator;
        private IInputService _inputService;

        [Inject]
        public void Construct(ISoundController soundController, IActionMediator actionMediator, IInputService inputService)
        {
            _inputService = inputService;
            _actionMediator = actionMediator;
            _soundController = soundController;
        }

        private void Start()
        {
            UpdateButtonObjectPosition(_offPositionZ);
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (_inputService.IsClickDown)
            {
                var ray = _hudCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit) && hit.collider.gameObject == _buttonObject.gameObject)
                {
                    Press();
                }
            }
            else if (_inputService.IsClickUp)
            {
                Release();
            }
        }

        private void Press()
        {
            UpdateButtonObjectPosition(_onPositionZ);

            _actionMediator.EnableNitro(_moveSpeedMultiplier);
            _soundController.PlaySfx(SfxType.Nitro);
        }

        private void Release()
        {
            UpdateButtonObjectPosition(_offPositionZ);

            _actionMediator.DisableNitro();
        }

        private void UpdateButtonObjectPosition(float positionZ)
        {
            _buttonObject.transform.localPosition = new Vector3(_buttonObject.transform.localPosition.x,
                _buttonObject.transform.localPosition.y, positionZ);
        }
    }
}