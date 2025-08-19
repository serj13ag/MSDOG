using Core.Controllers;
using Core.Sounds;
using Gameplay.Providers;
using UnityEngine;
using VContainer;

namespace UI.HUD.Actions
{
    public class NitroAction : MonoBehaviour
    {
        [SerializeField] private Camera _hudCamera;
        [SerializeField] private GameObject _buttonObject;
        [SerializeField] private float _moveSpeedMultiplier = 2f;
        [SerializeField] private float _offPositionZ = 1.4f;
        [SerializeField] private float _onPositionZ = 1f;

        private IPlayerProvider _playerProvider;
        private ISoundController _soundController;

        [Inject]
        public void Construct(IPlayerProvider playerProvider, ISoundController soundController)
        {
            _playerProvider = playerProvider;
            _soundController = soundController;
        }

        public void Init()
        {
            _buttonObject.transform.localPosition = new Vector3(_buttonObject.transform.localPosition.x,
                _buttonObject.transform.localPosition.y, _offPositionZ);
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _hudCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider.gameObject == _buttonObject.gameObject)
                    {
                        Press();
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                Unpress();
            }
        }

        private void Press()
        {
            _playerProvider.Player.SetNitro(_moveSpeedMultiplier);
            _soundController.PlaySfx(SfxType.Nitro);

            _buttonObject.transform.localPosition = new Vector3(_buttonObject.transform.localPosition.x,
                _buttonObject.transform.localPosition.y, _onPositionZ);
        }

        private void Unpress()
        {
            _playerProvider.Player.ResetNitro();
            _buttonObject.transform.localPosition = new Vector3(_buttonObject.transform.localPosition.x,
                _buttonObject.transform.localPosition.y, _offPositionZ);
        }
    }
}