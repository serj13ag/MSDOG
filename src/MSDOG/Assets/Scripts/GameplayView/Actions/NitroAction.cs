using Core.Controllers;
using Core.Sounds;
using UnityEngine;
using VContainer;

namespace GameplayView.Actions
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

        [Inject]
        public void Construct(ISoundController soundController, IActionMediator actionMediator)
        {
            _actionMediator = actionMediator;
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
            _buttonObject.transform.localPosition = new Vector3(_buttonObject.transform.localPosition.x,
                _buttonObject.transform.localPosition.y, _onPositionZ);

            _actionMediator.EnableNitro(_moveSpeedMultiplier);
            _soundController.PlaySfx(SfxType.Nitro);
        }

        private void Unpress()
        {
            _buttonObject.transform.localPosition = new Vector3(_buttonObject.transform.localPosition.x,
                _buttonObject.transform.localPosition.y, _offPositionZ);

            _actionMediator.DisableNitro();
        }
    }
}