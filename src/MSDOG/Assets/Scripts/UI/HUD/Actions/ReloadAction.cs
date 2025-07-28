using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.Actions
{
    public class ReloadAction : MonoBehaviour
    {
        [SerializeField] private Camera _hudCamera;
        [SerializeField] private GameObject _handleObject;
        [SerializeField] private Image _fillImage;
        [SerializeField] private float _unwindSpeed = 40f;
        [SerializeField] private float _startingAngleLerp = 0.5f;
        [SerializeField] private float _maxAngle = 1080f;

        private float _currentAngle;

        private bool _dragging;
        private float _currentDragAngle;
        private Player _player;

        public void Init(Player player)
        {
            _player = player;

            _currentAngle = Mathf.Lerp(0f, _maxAngle, _startingAngleLerp);
            UpdateFillImageView();
        }

        private void Update()
        {
            HandleInput();

            if (_dragging)
            {
                UpdateDragRotation();
            }
            else if (_currentAngle > 0f)
            {
                Unwind();
            }
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
                        _currentDragAngle = GetMouseAngle();
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _dragging = false;
            }
        }

        private void Unwind()
        {
            TryRotate(-Time.deltaTime * _unwindSpeed);
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
            _handleObject.transform.Rotate(Vector3.back, oldAngle - _currentAngle);
            UpdateFillImageView();
        }

        private void UpdateFillImageView()
        {
            var t = _currentAngle / _maxAngle;
            var color = t switch
            {
                < 0.6f => Color.gray,
                < 0.8f => Color.green,
                _ => Color.red,
            };
            _fillImage.fillAmount = t;
            _fillImage.color = color;
        }

        private float GetMouseAngle()
        {
            var objectScreenPos = _hudCamera.WorldToScreenPoint(_handleObject.transform.position);
            var dir = Input.mousePosition - objectScreenPos;
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
    }
}