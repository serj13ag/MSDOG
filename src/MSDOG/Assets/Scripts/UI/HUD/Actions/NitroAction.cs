using Core;
using Services;
using Sounds;
using UnityEngine;

namespace UI.HUD.Actions
{
    public class NitroAction : MonoBehaviour
    {
        [SerializeField] private Camera _hudCamera;
        [SerializeField] private GameObject _buttonObject;
        [SerializeField] private float _moveSpeedMultiplier = 2f;
        [SerializeField] private float _offPositionZ = 1.4f;
        [SerializeField] private float _onPositionZ = 1f;

        private Player _player;
        private SoundService _soundService;

        public void Init(Player player, SoundService soundService)
        {
            _soundService = soundService;
            _player = player;

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
            _player.SetNitro(_moveSpeedMultiplier);
            _soundService.PlaySfx(SfxType.Nitro);

            _buttonObject.transform.localPosition = new Vector3(_buttonObject.transform.localPosition.x,
                _buttonObject.transform.localPosition.y, _onPositionZ);
        }

        private void Unpress()
        {
            _player.ResetNitro();
            _buttonObject.transform.localPosition = new Vector3(_buttonObject.transform.localPosition.x,
                _buttonObject.transform.localPosition.y, _offPositionZ);
        }
    }
}